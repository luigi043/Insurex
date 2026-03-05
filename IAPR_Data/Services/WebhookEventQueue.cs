using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IAPR_Data.Classes;
using IAPR_Data.Classes.Webhook;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IAPR_Data.Services
{
    /// <summary>
    /// Thread-safe queue for decoupled webhook event processing.
    /// In .NET 8, this is registered as a Singleton and consumed by a BackgroundService.
    /// </summary>
    public sealed class WebhookEventQueue
    {
        private readonly ConcurrentQueue<WebhookEventMessage> _queue
            = new ConcurrentQueue<WebhookEventMessage>();

        private readonly ILogger<WebhookEventQueue> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public WebhookEventQueue(ILogger<WebhookEventQueue> logger, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        public Action<WebhookEventMessage>? OnMessage { get; set; }
        public Func<WebhookEventMessage, Task>? OnMessageAsync { get; set; }

        public void Enqueue(WebhookEventMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _queue.Enqueue(message);
        }

        public async Task ProcessNextAsync(CancellationToken ct)
        {
            if (_queue.TryDequeue(out var message))
            {
                try
                {
                    if (OnMessageAsync != null)
                        await OnMessageAsync(message);
                    else
                        OnMessage?.Invoke(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process event {EventId}", message.EventId);

                    try
                    {
                        using (var db = await _dbFactory.CreateDbContextAsync(ct))
                        {
                            var evt = await db.WebhookEvents
                                       .FirstOrDefaultAsync(e => e.EventId == message.EventId, ct);
                            if (evt != null)
                            {
                                evt.Status = "Failed";
                                evt.ProcessingError = ex.Message;
                                await db.SaveChangesAsync(ct);
                            }
                        }
                    }
                    catch (Exception dbEx)
                    {
                        _logger.LogError(dbEx, "Failed to log processing error to DB for event {EventId}", message.EventId);
                    }
                }
            }
        }

        public bool IsEmpty => _queue.IsEmpty;
    }

    public class WebhookEventMessage
    {
        public string EventId { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public int? TenantId { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}







