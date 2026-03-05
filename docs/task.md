# Task Checklist

## Platform Modernization & Missing Features (Roadmap)

Based on the provided documents (`Asset Protection Register - product write-up`, `InsureX Platform - System Description`, and `InsureX - ChatGPT Code review`), the following features and architectural components are missing from the current implementation:

### Phase 0: Emergency Security Remediation

- [x] Move hardcoded credentials (SMTP, DB, Payment Gateway) to Azure Key Vault or Environment Variables.
- [x] Add a [.gitignore](file:///c:/Users/cluiz/projectx/Insurex/.gitignore) file and remove `bin/`, `obj/`, and `.vs/` directories from source control.
- [x] Remove hardcoded AES keys from [CryptorEngine.cs](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Data/Utils/CryptorEngine.cs).
- [x] Enforce HTTPS and secure cookie settings.

### Phase 1: Authentication & Data Protection

- [x] Replace the custom session-based authentication with ASP.NET Core Identity (PBKDF2/Argon2 hashed passwords).
- [x] Implement centralized Role-Based Access Control (RBAC) (Admin, BankUser, InsurerUser).
- [x] Implement secure AES-GCM encryption for sensitive application data.

### Phase 2: Multi-Tenant Architecture

- [x] Introduce a `Tenant` and `Organization` entity model.
- [x] Add `TenantId` to all relevant domain entities (Assets, Policies, Cases).
- [x] Implement `TenantContext` middleware to resolve the current tenant from the request.
- [x] Enforce tenant isolation using EF Core Global Query Filters.
- [x] Implement SQL Server Row-Level Security (RLS) for strict database isolation.

### Phase 3: Event-Driven Compliance Engine

- [x] Build secure webhook ingestion endpoints for insurers (HMAC signature validation, replay protection, idempotency).
- [x] Integrate a message queue (Azure Service Bus or RabbitMQ) for decoupled event processing.
- [x] Develop the Compliance Engine service to deterministically evaluate rules and emit compliance outcomes (`ComplianceState`).
- [x] Implement the Outbox pattern to ensure atomic database writes and reliable event publishing.

### Phase 4: Workflow Orchestration & Audit Trails

- [x] Create an immutable Audit Log / Evidence Ledger (capturing who, what, when, old/new values, and `CorrelationId`).
- [x] Implement Case Management and Workflow Orchestration (tasks, SLA timers, escalations).
- [x] Refactor the [Notification_Provider](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Data/Providers/Notification_Provider.cs#19-814) to send targeted, templated emails/SMS securely with a robust retry policy (addressing the current bulk BCC/CC leaks).

### Phase 5: API & Frontend Modernization

- [x] Build structured RESTful JSON APIs for all modules with standard pagination envelopes.
- [x] Implement OAuth2 Client Credentials for secure system-to-system integrations (Banks and Insurers).
- [x] Migrate the legacy WebForms UI (`IAPR_Web`) to ASP.NET Core MVC (Razor + jQuery partials) or a modern SPA framework (React).

### Phase 9: Administrative Controls

- [x] Implement User Management CRUD (Create, Edit, Role assignment).
- [x] Implement Tenant/Entity Management (Financers, Insurers).
- [x] Add multi-tenant context awareness to administrative endpoints.

### Phase 10: Advanced Intelligence & Bulk Operations

- [ ] Implement Bulk Asset Import service (processing CSV/Excel via `Bulk_Import_Provider`).
- [ ] Build Intelligence API for risk scoring and compliance trends.
- [ ] Create Intelligence Dashboard in React with interactive data visualizations.
- [ ] Implement OAuth2 Client Credentials for secure partner integrations.

## Completed Tasks

- [x] Fix IAPR_Web Build Errors
  - [x] Restore NuGet packages for IAPR_Web
  - [x] Install missing AjaxControlToolkit dependency via NuGet
  - [x] Fix FusionCharts and other missing reference paths
  - [x] Verify IAPR_Web compiles successfully
- [x] Start the Project
  - [x] Run the web application using IIS Express or dotnet run
