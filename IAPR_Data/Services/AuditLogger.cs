using System;
using IAPR_Data.Classes;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace IAPR_Data.Services
{
    /// <summary>
    /// Thin helper for writing immutable <see cref="AuditLogEntry"/> rows.
    /// Call inside the same EF transaction as the domain change — no separate commit.
    /// </summary>
    public static class AuditLogger
    {
        /// <summary>
        /// Appends an audit entry to the provided <paramref name="db"/> context.
        /// The caller is responsible for calling <c>db.SaveChanges()</c>.
        /// </summary>
        public static void Log(
            ApplicationDbContext db,
            string entityName,
            string? entityId,
            string? action,
            object? oldValues    = null,
            object? newValues    = null,
            string? actorUserId  = null,
            string? actorName    = null,
            int?   tenantId     = null,
            string? correlationId = null,
            string? notes        = null)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            var entry = new AuditLogEntry
            {
                CorrelationId = correlationId ?? Guid.NewGuid().ToString("N"),
                EntityName    = entityName    ?? "Unknown",
                EntityId      = entityId ?? "0",
                Action        = action        ?? "Unknown",
                OldValues     = oldValues != null ? JsonConvert.SerializeObject(oldValues) : null,
                NewValues     = newValues != null ? JsonConvert.SerializeObject(newValues) : null,
                ActorUserId   = actorUserId,
                ActorName     = actorName     ?? "System",
                TenantId      = tenantId,
                Notes         = notes,
                OccurredAt     = DateTime.UtcNow
            };

            db.AuditLog.Add(entry);
        }

        /// <summary>Convenience overload that writes and saves in one call (managed context).</summary>
        public static void LogStandalone(
            ApplicationDbContext db,
            string entityName,
            string entityId,
            string action,
            object? newValues    = null,
            string? actorName    = null,
            int?   tenantId     = null,
            string? correlationId = null,
            string? notes        = null)
        {
            Log(db, entityName, entityId, action,
                newValues: newValues,
                actorName: actorName ?? "System",
                tenantId: tenantId,
                correlationId: correlationId,
                notes: notes);
            db.SaveChanges();
        }
    }
}







