using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IAPR_Data.Classes;
using IAPR_Data.Classes.Webhook;

namespace InsureX.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires valid JWT
    public class PartnerIntegrationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public PartnerIntegrationController(ApplicationDbContext db)
        {
            _db = db;
        }

        private int? GetCurrentTenantId()
        {
            var tid = User.FindFirst("TenantId")?.Value;
            return tid != null ? int.Parse(tid) : null;
        }

        [HttpGet("webhooks")]
        public async Task<ActionResult<IEnumerable<PartnerWebhookConfig>>> GetWebhooks()
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId == null) return Unauthorized();

            return await _db.PartnerWebhookConfigs
                .Where(c => c.TenantId == tenantId)
                .ToListAsync();
        }

        [HttpPost("webhooks")]
        public async Task<ActionResult<PartnerWebhookConfig>> CreateWebhook(PartnerWebhookConfig config)
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId == null) return Unauthorized();

            config.TenantId = tenantId.Value;
            config.UpdatedAt = DateTime.UtcNow;
            
            _db.PartnerWebhookConfigs.Add(config);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWebhooks), new { id = config.Id }, config);
        }

        [HttpPut("webhooks/{id}")]
        public async Task<IActionResult> UpdateWebhook(int id, PartnerWebhookConfig config)
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId == null) return Unauthorized();

            if (id != config.Id) return BadRequest();

            var existing = await _db.PartnerWebhookConfigs
                .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);

            if (existing == null) return NotFound();

            existing.TargetUrl = config.TargetUrl;
            existing.IsActive = config.IsActive;
            existing.SubscribedEvents = config.SubscribedEvents;
            existing.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("webhooks/{id}/rotate-secret")]
        public async Task<ActionResult<string>> RotateSecret(int id)
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId == null) return Unauthorized();

            var existing = await _db.PartnerWebhookConfigs
                .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);

            if (existing == null) return NotFound();

            existing.Secret = Guid.NewGuid().ToString("N");
            existing.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(existing.Secret);
        }

        [HttpPost("webhooks/{id}/test")]
        public async Task<IActionResult> TestWebhook(int id)
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId == null) return Unauthorized();

            var config = await _db.PartnerWebhookConfigs
                .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);

            if (config == null) return NotFound();
            if (string.IsNullOrEmpty(config.TargetUrl)) return BadRequest("Target URL not configured.");

            var payload = new
            {
                EventId = Guid.NewGuid().ToString(),
                EventType = "ping",
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Data = new { Message = "Test webhook from InsureX" }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            var timestampStr = payload.Timestamp.ToString();

            using var httpClient = new System.Net.Http.HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            
            using var requestMessage = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, config.TargetUrl);
            requestMessage.Content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

            using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(config.Secret));
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(json));
            var signature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

            requestMessage.Headers.Add("X-InsureX-Signature", signature);
            requestMessage.Headers.Add("X-InsureX-Timestamp", timestampStr);
            requestMessage.Headers.Add("X-InsureX-Event-Id", payload.EventId);

            try
            {
                var response = await httpClient.SendAsync(requestMessage);
                return Ok(new 
                { 
                    Success = response.IsSuccessStatusCode, 
                    StatusCode = (int)response.StatusCode,
                    ResponseBody = await response.Content.ReadAsStringAsync()
                });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Error = ex.Message });
            }
        }

        [HttpDelete("webhooks/{id}")]
        public async Task<IActionResult> DeleteWebhook(int id)
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId == null) return Unauthorized();

            var existing = await _db.PartnerWebhookConfigs
                .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);

            if (existing == null) return NotFound();

            _db.PartnerWebhookConfigs.Remove(existing);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
