using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using IAPR_Data.Classes;
using IAPR_Data.Classes.Webhook;

namespace IAPR_Data.Services
{
    /// <summary>
    /// Lightweight in-process message queue for decoupled webhook event processing.
    /// Acts as the message bus between the WebhookService (producer) and the ComplianceEngine (consumer).
    /// Uses a ConcurrentQueue + dedicated background thread — compatible with .NET Framework 4.8.
    /// </summary>
    public sealed class WebhookEventQueue : IDisposable
    {
        private static readonly Lazy<WebhookEventQueue> _instance =
            new Lazy<WebhookEventQueue>(() => new WebhookEventQueue());

        /// <summary>Singleton access point</summary>
        public static WebhookEventQueue Instance => _instance.Value;

        private readonly ConcurrentQueue<WebhookEventMessage> _queue
            = new ConcurrentQueue<WebhookEventMessage>();

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Thread _workerThread;
        private volatile bool _isRunning;

        // Pluggable processor delegate — set by the compliance engine
        public Action<WebhookEventMessage> OnMessage { get; set; }

        private WebhookEventQueue()
        {
            _workerThread = new Thread(ProcessLoop)
            {
                IsBackground = true,
                Name = "WebhookEventQueue-Worker"
            };
        }

        /// <summary>Starts the background processing loop</summary>
        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            _workerThread.Start();
        }

        /// <summary>Enqueues a webhook event message for async processing</summary>
        public void Enqueue(WebhookEventMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _queue.Enqueue(message);
        }

        private void ProcessLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var message))
                {
                    try
                    {
                        OnMessage?.Invoke(message);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(
                            $"[WebhookEventQueue] Failed to process event {message?.EventId}: {ex.Message}");

                        // Mark event as Failed in the DB
                        try
                        {
                            using (var db = ApplicationDbContext.Create())
                            {
                                var evt = db.WebhookEvents
                                           .FirstOrDefault(e => e.EventId == message.EventId);
                                if (evt != null)
                                {
                                    evt.Status = "Failed";
                                    evt.ProcessingError = ex.Message;
                                    db.SaveChanges();
                                }
                            }
                        }
                        catch { /* db error during error logging — swallow */ }
                    }
                }
                else
                {
                    // Queue is empty — rest the thread briefly
                    Thread.Sleep(200);
                }
            }
        }

        public void Stop() => _cts.Cancel();

        public void Dispose()
        {
            Stop();
            _cts.Dispose();
        }
    }

    /// <summary>
    /// Lightweight message envelope passed through the in-process queue.
    /// </summary>
    public class WebhookEventMessage
    {
        public string EventId { get; set; }
        public string Source { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public int? TenantId { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}
