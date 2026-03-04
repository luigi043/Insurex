using System;
using System.Linq;
using System.Threading;
using IAPR_Data.Classes;

namespace IAPR_Data.Services
{
    /// <summary>
    /// Outbox pattern publisher.
    /// Runs as a background thread, polling the <see cref="OutboxMessage"/> table for
    /// unpublished messages, dispatching them to downstream subscribers, and marking
    /// them as published upon success.
    ///
    /// The current transport is diagnostic tracing — swap in Azure Service Bus or
    /// RabbitMQ by replacing the body of <see cref="Publish"/> without touching
    /// the orchestration logic.
    ///
    /// Call <see cref="Start"/> once from Application_Start.
    /// </summary>
    public sealed class OutboxPublisher : IDisposable
    {
        private static readonly Lazy<OutboxPublisher> _instance =
            new Lazy<OutboxPublisher>(() => new OutboxPublisher());

        /// <summary>Singleton access point.</summary>
        public static OutboxPublisher Instance => _instance.Value;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Thread _workerThread;
        private volatile bool _isRunning;

        /// <summary>Poll interval in milliseconds (default: 5 seconds).</summary>
        private const int PollIntervalMs = 5000;

        /// <summary>Maximum publish attempts before a message is abandoned.</summary>
        private const int MaxAttempts = 5;

        private OutboxPublisher()
        {
            _workerThread = new Thread(PollLoop)
            {
                IsBackground = true,
                Name = "OutboxPublisher-Worker"
            };
        }

        /// <summary>Starts the background publisher thread.</summary>
        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            _workerThread.Start();
            System.Diagnostics.Trace.TraceInformation("[OutboxPublisher] Started.");
        }

        public void Stop() => _cts.Cancel();

        public void Dispose()
        {
            Stop();
            _cts.Dispose();
        }

        // ------------------------------------------------------------------
        // Background poll loop
        // ------------------------------------------------------------------

        private void PollLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    PublishPending();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("[OutboxPublisher] Poll loop error: " + ex.Message);
                }

                Thread.Sleep(PollIntervalMs);
            }
        }

        private void PublishPending()
        {
            using (var db = ApplicationDbContext.Create())
            {
                // Fetch up to 50 unpublished messages ordered by creation time
                var pending = db.OutboxMessages
                    .Where(m => m.PublishedAt == null && m.AttemptCount < MaxAttempts)
                    .OrderBy(m => m.CreatedAt)
                    .Take(50)
                    .ToList();

                if (!pending.Any()) return;

                foreach (var msg in pending)
                {
                    try
                    {
                        // --- Transport layer: replace this with Azure Service Bus / RabbitMQ publish ---
                        Publish(msg);

                        msg.PublishedAt = DateTime.UtcNow;
                        msg.AttemptCount++;
                        msg.LastError = null;
                    }
                    catch (Exception ex)
                    {
                        msg.AttemptCount++;
                        msg.LastError = ex.Message;
                        System.Diagnostics.Trace.TraceError(
                            $"[OutboxPublisher] Failed to publish message {msg.Id} (attempt {msg.AttemptCount}): {ex.Message}");
                    }
                }

                db.SaveChanges();
            }
        }

        // ------------------------------------------------------------------
        // Transport stub — replace with real message bus integration
        // ------------------------------------------------------------------

        /// <summary>
        /// Publishes a single outbox message to downstream subscribers.
        /// Currently writes to the diagnostic trace; swap in a real transport here.
        /// </summary>
        private static void Publish(OutboxMessage message)
        {
            // TODO: Replace with Azure Service Bus, RabbitMQ, or SignalR push:
            // e.g.:  serviceBusClient.SendMessageAsync(new ServiceBusMessage(message.Payload));
            System.Diagnostics.Trace.TraceInformation(
                $"[OutboxPublisher] Published: type={message.MessageType} | correlationId={message.CorrelationId} | payload={message.Payload}");
        }
    }
}
