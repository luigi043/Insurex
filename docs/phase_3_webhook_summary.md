# Phase 3: Event-Driven Compliance Engine

## Part 1: Secure Webhook Ingestion Endpoints

### Objective
Build a production-grade webhook ingestion layer that insurers can use to push events (e.g. `policy.created`, `claim.submitted`) into InsureX. The endpoint must resist spoofing, replay attacks, and duplicate event processing.

### Files Created

| File | Purpose |
|---|---|
| `IAPR_Data/Classes/Webhook/WebhookModels.cs` | EF entity `WebhookEvent` — persists every received event for audit and async processing |
| `IAPR_Data/Classes/Webhook/HmacValidator.cs` | `HmacValidator.IsValid()` — constant-time HMAC-SHA256 comparison preventing timing attacks |
| `IAPR_API/WebhookService.svc.cs` | WCF REST webhook endpoint at `POST /webhook/insurers/{source}/events` |

### Security Features Implemented

1. **HMAC-SHA256 Signature Validation:** Every incoming request must carry an `X-Signature-SHA256` header. The raw request body is signed by the insurer's shared secret. We re-compute the HMAC and compare using constant-time equals to prevent timing-based side-channel attacks. Supports `sha256=` prefix format (GitHub/Stripe style).

2. **Replay Attack Protection:** Reads the `X-Event-Timestamp` header and rejects any request where the event timestamp is more than 5 minutes old or in the future.

3. **Idempotency:** Reads the `X-Event-ID` header. Before processing, we check the `WebhookEvents` EF table to see if this EventId was already processed. Duplicate events return HTTP 200 with `"already processed"` instead of re-processing.

4. **Audit Persistence:** All received webhook events are persisted to the `WebhookEvents` EF table with status `Pending`, enabling asynchronous processing and full audit trail.

5. **Tenant Scoping:** The stored `WebhookEvent` includes `TenantId` from `TenantContext.Current`, so events are always scoped to the authenticated tenant.

### Result
`IAPR_Data` compiles with **0 errors** with the new webhook models and HMAC utility.
