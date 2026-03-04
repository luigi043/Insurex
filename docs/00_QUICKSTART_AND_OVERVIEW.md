# InsureX Platform – Project Kickoff & Quick Reference
## ASP.NET Core 8 | React/TypeScript | SQL Server 2022 | Multi-Tenant B2B SaaS

---

## 📋 EXECUTIVE OVERVIEW

You have **three comprehensive guides** plus this kickoff document:

1. **InsureX_Implementation_Guide.md** (55 KB) – Complete architecture, 9 phases, full code examples
2. **SQL_Server_Setup_Guide.md** (18 KB) – Database design, RLS, migrations, maintenance
3. **React_TypeScript_Frontend_Guide.md** (30 KB) – Frontend architecture, components, API clients

**Recommended reading order:** This document → Implementation Guide → SQL & Frontend guides

---

## 🚀 QUICK START (This Week)

### Phase 0 – Security (IMMEDIATE)

**Your current system has critical issues. Fix these first:**

1. **Remove secrets from code** (2 hours)
   ```bash
   # Remove exposed credentials from Web.config
   # Delete bin/, obj/, .vs/ directories
   # Run: git filter-branch --tree-filter 'rm -rf obj bin .vs' HEAD
   ```

2. **Create .gitignore** (30 minutes)
   ```gitignore
   bin/
   obj/
   .vs/
   .vscode/settings.json
   .env
   .env.local
   appsettings.Development.json
   Web.config
   ```

3. **Rotate all credentials** (1 hour)
   - SMTP passwords
   - Database passwords
   - Payment gateway keys
   - API keys

4. **Set up Azure Key Vault** (2 hours)
   - For prod/staging secrets
   - Local dev uses `.env` + environment variables

**⏱️ Total time: 4–6 hours**

---

## 🛠️ TECH STACK CONFIRMATION

### Backend
- **Runtime:** .NET 8.0 (latest LTS)
- **Framework:** ASP.NET Core 8 MVC + Web API
- **Database:** SQL Server 2022 (on localdb: `(localdb)\MSSQLLocalDB`)
- **ORM:** Entity Framework Core 8.0
- **Auth:** JWT + ASP.NET Identity
- **Container:** Docker + Docker Desktop

### Frontend
- **Runtime:** Node.js 18+
- **Framework:** React 18+ with TypeScript
- **API Client:** Axios
- **State:** Zustand (lightweight alternative to Redux)
- **Styling:** Tailwind CSS
- **Router:** React Router v6

### DevOps
- **IDE:** Visual Studio 2022 (backend) + VS Code (frontend)
- **DB Admin:** SQL Server Management Studio 22
- **Build:** GitHub Actions / Azure DevOps
- **Container Registry:** Docker Hub / Azure Container Registry

---

## 📁 YOUR DIRECTORY STRUCTURE (After Setup)

```
InsureX/
├── src/
│   ├── InsureX.Web                (ASP.NET Core MVC – Razor views)
│   ├── InsureX.Api                (ASP.NET Core Web API)
│   ├── InsureX.Application        (Business services, rules)
│   ├── InsureX.Domain             (Entities, enums, interfaces)
│   ├── InsureX.Infrastructure     (EF Core, SQL, repos, integrations)
│   ├── InsureX.Shared             (DTOs, exceptions, constants)
│   └── InsureX.Frontend           (React/TypeScript SPA)
├── tests/
│   ├── InsureX.UnitTests
│   └── InsureX.IntegrationTests
├── docker/
│   ├── Dockerfile.api
│   ├── Dockerfile.web
│   └── docker-compose.yml
├── sql/
│   ├── migrations/
│   ├── seed-data.sql
│   └── stored-procedures.sql
├── docs/
│   ├── API.md
│   ├── ARCHITECTURE.md
│   ├── SECURITY.md
│   └── SETUP.md
├── .env.example
├── .gitignore
├── docker-compose.yml
└── InsureX.sln
```

---

## 💾 DATABASE SETUP (SQL SERVER)

### 1. Verify LocalDB Connection

**In SQL Server Management Studio (SSMS 22):**
- Server: `(localdb)\MSSQLLocalDB`
- Auth: Windows Authentication

```sql
-- Test connection
SELECT @@VERSION, @@SERVICENAME;
```

### 2. Create Database

```sql
CREATE DATABASE InsureXDb
    COLLATE Latin1_General_100_CI_AS;
```

### 3. Connection String (ASP.NET Core)

**appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InsureXDb;Integrated Security=True;Encrypt=False;"
  }
}
```

### 4. Apply Migrations

```bash
cd src/InsureX.Api
dotnet ef database update
```

✅ Done. Database is ready.

---

## 🏗️ SOLUTION CREATION (1 Day)

### Step 1: Create Solution
```bash
dotnet new sln -n InsureX
cd InsureX
```

### Step 2: Create Projects
```bash
# Core projects
dotnet new webapi -n InsureX.Api -f net8.0
dotnet new classlib -n InsureX.Application -f net8.0
dotnet new classlib -n InsureX.Domain -f net8.0
dotnet new classlib -n InsureX.Infrastructure -f net8.0
dotnet new classlib -n InsureX.Shared -f net8.0

# Add to solution
dotnet sln InsureX.sln add src/**/*.csproj tests/**/*.csproj
```

### Step 3: Add NuGet Packages

**In InsureX.Api:**
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Serilog.AspNetCore
dotnet add package Azure.Identity
dotnet add package Azure.Security.KeyVault.Secrets
```

### Step 4: Set Up Multi-Tenancy

Follow **Phase 2** of the Implementation Guide:
- Create `ITenantContext` interface
- Implement `TenantContext` class
- Add EF Core global query filters
- (Optional) Set up SQL Server RLS

### Step 5: Create Domain Models

Follow **Phase 4** of the Implementation Guide:
- Asset, Policy, Borrower (Registry)
- ComplianceState, ComplianceDecision (Compliance)
- NonComplianceCase, CaseTask (Workflow)
- AuditLog, EventEnvelope (Audit)

### Step 6: Create First Migration

```bash
cd src/InsureX.Infrastructure
dotnet ef migrations add InitialCreate -s ../InsureX.Api
dotnet ef database update
```

✅ You now have a working ASP.NET Core + SQL Server project.

---

## 🌐 API ENDPOINTS (RESTful)

### Assets
```
GET    /api/v1/assets              (paged list)
GET    /api/v1/assets/{id}         (single)
POST   /api/v1/assets              (create)
PUT    /api/v1/assets/{id}         (update)
POST   /api/v1/assets/import       (bulk import)
```

### Policies
```
GET    /api/v1/policies            (paged list)
GET    /api/v1/assets/{id}/policies
POST   /api/v1/policies            (create)
```

### Compliance
```
GET    /api/v1/compliance/assets            (paged states)
GET    /api/v1/assets/{id}/compliance       (detail + decisions)
GET    /api/v1/assets/{id}/compliance/decisions
```

### Cases (Workflow)
```
GET    /api/v1/cases               (paged list)
GET    /api/v1/cases/{id}          (single)
POST   /api/v1/cases/{id}/actions/escalate
POST   /api/v1/cases/{id}/actions/close
```

### Audit
```
GET    /api/v1/audit               (paged log)
GET    /api/v1/evidence/{id}       (retrieve payload)
```

### Integrations
```
POST   /api/v1/integrations/insurers/{code}/webhook   (signed)
POST   /api/v1/integrations/banks/{code}/assets
```

---

## 🎨 REACT FRONTEND SETUP (1 Day)

### Step 1: Create React App
```bash
# Option A: Create-React-App
npx create-react-app InsureX.Frontend --template typescript

# Option B: Vite (faster)
npm create vite@latest InsureX.Frontend -- --template react-ts
```

### Step 2: Install Dependencies
```bash
npm install axios zustand react-router-dom
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
```

### Step 3: Create Folder Structure
```
src/
├── api/
│   ├── clients/
│   │   ├── assetClient.ts
│   │   ├── complianceClient.ts
│   │   ├── caseClient.ts
│   │   └── authClient.ts
│   ├── types/
│   │   ├── Common.ts
│   │   ├── Asset.ts
│   │   ├── Compliance.ts
│   │   └── Case.ts
│   └── utils/
│       └── api.ts
├── components/
│   ├── layouts/
│   ├── pages/
│   ├── tables/
│   ├── forms/
│   └── shared/
├── hooks/
├── stores/
├── utils/
└── styles/
```

### Step 4: Copy Code from React Guide

All component code is in **React_TypeScript_Frontend_Guide.md**:
- API clients with interceptors
- Zustand stores for auth/tenant
- Login, Assets, Compliance, Cases pages
- Pagination, status badges, tables
- Complete routing setup

### Step 5: Start Dev Server
```bash
npm run dev
```

Visit `http://localhost:5173` (Vite) or `http://localhost:3000` (CRA)

✅ React app is running.

---

## 🔐 SECURITY CHECKLIST

- [ ] **Phase 0 fixes**
  - [ ] Remove secrets from repo
  - [ ] Update .gitignore
  - [ ] Rotate credentials

- [ ] **Authentication**
  - [ ] Implement JWT tokens
  - [ ] Add refresh token logic
  - [ ] Secure cookie flags (Secure, HttpOnly, SameSite)
  - [ ] HTTPS everywhere (even local dev with self-signed cert)

- [ ] **Database**
  - [ ] Enable SQL Server Row-Level Security (RLS)
  - [ ] Configure user account permissions (least privilege)
  - [ ] Encrypt backups
  - [ ] Set up audit logging

- [ ] **API Security**
  - [ ] Validate all inputs
  - [ ] Enforce pagination (max 100 items)
  - [ ] Rate limiting on webhooks
  - [ ] CORS configured properly
  - [ ] CSRF tokens on form submissions

- [ ] **Secrets Management**
  - [ ] No secrets in code
  - [ ] Use Azure Key Vault (prod)
  - [ ] Use .env files locally (git-ignored)
  - [ ] Rotate keys regularly

---

## 📊 DEVELOPMENT TIMELINE (Recommended)

| Phase | Duration | Focus |
|-------|----------|-------|
| 0 | Week 1 | Emergency security fixes |
| 1 | Week 2 | New ASP.NET Core solution + structure |
| 2 | Weeks 3–4 | Multi-tenancy + EF Core setup |
| 3 | Weeks 5–6 | Authentication + JWT |
| 4 | Weeks 7–8 | Domain models + migrations |
| 5 | Weeks 9–11 | Business services (rules, workflow) |
| 6 | Weeks 12–14 | REST API endpoints |
| 7 | Weeks 15–17 | React frontend |
| 8 | Weeks 18–19 | Database + seeding |
| 9 | Weeks 20–21 | Docker + deployment |
| Testing | Weeks 22–24 | Unit + integration + system tests |
| **Total** | **~24 weeks** | MVP ready |

*Adjust timeline based on team size and complexity.*

---

## 🧪 TESTING STRATEGY

### Unit Tests (Services, Rules)
```bash
dotnet new xunit -n InsureX.UnitTests
dotnet add package Moq xunit
```

Example test:
```csharp
[Fact]
public async Task EvaluateAsset_WithNoPolicies_ReturnsNonCompliant()
{
    // Arrange
    var service = new ComplianceEvaluationService(...);
    var assetId = Guid.NewGuid();
    
    // Act
    var decision = await service.EvaluateAssetAsync(assetId);
    
    // Assert
    Assert.Equal(ComplianceStatus.NonCompliant, decision.NewStatus);
}
```

### Integration Tests (DB, EF Core)
```bash
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

### API Tests
```bash
dotnet add package WebApplicationFactory
```

---

## 🐳 DOCKER & DEPLOYMENT

### Local Docker Compose
```bash
docker-compose up -d
```

Spins up:
- SQL Server container
- ASP.NET Core API
- React frontend

### Push to Azure Container Registry
```bash
az acr build --registry <registry-name> --image insurex-api:latest .
```

### Deploy to Azure App Service
```bash
az webapp create --resource-group <rg> --plan <plan> --name insurex-api --deployment-container-image-name <image>
```

---

## 📞 SUPPORT & RESOURCES

### Documentation
- **InsureX_Implementation_Guide.md** – Full architecture + all code
- **SQL_Server_Setup_Guide.md** – Database design, RLS, stored procs
- **React_TypeScript_Frontend_Guide.md** – Frontend complete guide

### External References
- [ASP.NET Core 8 Docs](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/)
- [React TypeScript Handbook](https://www.typescriptlang.org/docs/handbook/react.html)
- [SQL Server RLS](https://learn.microsoft.com/en-us/sql/relational-databases/security/row-level-security)

### Tools
- Visual Studio 2022 – Backend IDE
- VS Code – Frontend IDE + scripting
- SQL Server Management Studio 22 – Database admin
- Docker Desktop – Containers
- Postman / Insomnia – API testing

---

## ✅ IMMEDIATE ACTION ITEMS

### This Week (Security Week)
- [ ] Review Phase 0 security fixes
- [ ] Remove secrets from repo
- [ ] Add .gitignore
- [ ] Rotate credentials
- [ ] Create Azure Key Vault (if using Azure)

### Next Week (Solution Week)
- [ ] Create new ASP.NET Core 8 solution
- [ ] Set up project structure (6 projects)
- [ ] Add NuGet packages
- [ ] Implement TenantContext + multi-tenancy
- [ ] Create initial domain models
- [ ] Run first EF Core migration

### Week 3 (Frontend Week)
- [ ] Create React + TypeScript project
- [ ] Set up API client + types
- [ ] Build authentication flow
- [ ] Create main layout + navigation
- [ ] Build first few pages (assets, compliance)

### Ongoing
- [ ] Daily standup (if team)
- [ ] Code reviews
- [ ] Unit tests alongside development
- [ ] Automated CI/CD pipeline setup

---

## 🎯 SUCCESS CRITERIA

By **end of Week 1:**
- ✅ Secrets removed from code
- ✅ New clean .gitignore
- ✅ Credentials rotated

By **end of Week 4:**
- ✅ Clean ASP.NET Core solution
- ✅ Multi-tenancy foundation
- ✅ Domain models
- ✅ First database migration working

By **end of Week 8:**
- ✅ All core services working
- ✅ Compliance rules engine
- ✅ Workflow orchestration
- ✅ 80% unit test coverage

By **end of Week 12:**
- ✅ Complete REST API
- ✅ Signed webhook ingestion
- ✅ Paging + filtering everywhere

By **end of Week 16:**
- ✅ React frontend functional
- ✅ Login page working
- ✅ Asset + Compliance + Cases pages
- ✅ Pagination + filtering

By **end of Week 24:**
- ✅ MVP launch-ready
- ✅ Docker containers working
- ✅ SQL Server production-ready
- ✅ API docs generated
- ✅ Deployment pipeline automated

---

## 📝 DOCUMENT MAP

| Document | Size | Focus |
|----------|------|-------|
| **This File** | 5 KB | Quick start + overview |
| InsureX_Implementation_Guide.md | 55 KB | Full architecture (9 phases) |
| SQL_Server_Setup_Guide.md | 18 KB | Database design + RLS + migrations |
| React_TypeScript_Frontend_Guide.md | 30 KB | Frontend complete guide |

---

## 🔄 PROCESS WORKFLOW

1. **Read this document** (15 min) – Understand overview
2. **Read Implementation Guide** (60 min) – Deep dive into architecture
3. **Read SQL Guide** (30 min) – Database planning
4. **Read Frontend Guide** (30 min) – API + React setup
5. **Start Phase 0** (4–6 hours) – Security fixes
6. **Start Phase 1** (1–2 days) – Create solution
7. **Iterate through phases** – Follow timeline above

---

## 💡 KEY ARCHITECTURAL PRINCIPLES

### 1. Multi-Tenancy First
- Every table has `TenantId`
- TenantContext resolved from JWT claims
- EF Core global filters + SQL Server RLS

### 2. Event-Driven Compliance
- Insurer data → normalized events
- Rules engine evaluates → compliance decision
- Decision → workflow orchestration
- All immutable audit trail

### 3. Security by Default
- JWT tokens + roles
- Signed webhooks (HMAC)
- No secrets in code
- Field-level encryption (for PII)
- TLS everywhere

### 4. Scalable from Day 1
- Modular monolith (split later)
- Service Bus ready
- Outbox pattern for events
- Durable queue processing

---

## 🚨 COMMON PITFALLS (AVOID THESE)

❌ **Don't:**
- Commit .env or appsettings.json with secrets
- Use `IDENTITY_INSERT` in production
- Forget tenant filters in queries
- Mix business logic in controllers
- Use `SELECT *`
- Trust client-provided tenant ID
- Skip unit tests
- Deploy without code review
- Use reversible password encryption
- Hardcode API keys

✅ **Do:**
- Use Azure Key Vault or encrypted config
- Use EF Core for schema changes
- Always include TenantId in WHERE clauses
- Keep services focused
- Be explicit with columns
- Resolve tenant from JWT claims
- Test behavior early
- Require peer review
- Use bcrypt/Argon2 for passwords
- Rotate keys regularly

---

## 📞 QUESTIONS? NEXT STEPS?

1. **Start with Phase 0** – Security fixes
2. **Follow the Implementation Guide** – Phase by phase
3. **Reference SQL Guide** – For database-specific needs
4. **Reference Frontend Guide** – For React/TypeScript details
5. **Test early & often** – Don't wait until end to test

---

**Version:** 1.0  
**Date:** January 2026  
**Status:** Ready for Implementation

**All guides are in `/mnt/user-data/outputs/`**
