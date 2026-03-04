# InsureX Platform – Implementation Guide
## ASP.NET Core 8 | React/TypeScript | SQL Server | Multi-Tenant B2B SaaS

**Your Tech Stack:**
- Visual Studio 2022 (Modern IDE)
- Visual Studio Code (for frontend/scripts)
- SQL Server Management Studio 22
- SQL Server (LocalDB): `(localdb)\MSSQLLocalDB`
- ASP.NET Core (backend)
- React + TypeScript (frontend)
- Docker Desktop (containerization)

---

## EXECUTIVE SUMMARY

Your current codebase (WebForms + WCF) is a v1 system. To meet the InsureX product vision—a modern, secure, multi-tenant B2B SaaS platform for insurance compliance monitoring—you need a **structural rewrite** using ASP.NET Core + React. This guide provides a practical, phased implementation roadmap.

### Critical Issues (from code review)
1. **Hardcoded secrets** in Web.config files
2. **Weak authentication** (no Forms Auth, session-based with no CSRF protection)
3. **Insecure cryptography** (TripleDES, hardcoded keys, reversible passwords)
4. **No multi-tenancy model** (no tenant isolation)
5. **API layer is mostly stubbed** (WCF with DoWork() placeholders)
6. **No event-driven compliance detection** (synchronous, stored-proc heavy)

### What You're Building
A **multi-tenant SaaS control layer** that:
- Continuously monitors insurance compliance on financed assets
- Detects non-compliance in near real-time
- Orchestrates engagement workflows (SMS/email/calls)
- Maintains immutable audit trails
- Supports bank + insurer dashboards
- Is bank/insurer-grade secure

---

## PHASE 0 – EMERGENCY SECURITY FIXES (Week 1)

### Immediate Actions
1. **Rotate ALL exposed credentials**
   - SMTP passwords (from Web.config)
   - Database passwords
   - Payment gateway credentials
   - API keys

2. **Remove secrets from repo**
   - Delete `bin/`, `obj/`, `.vs/` directories
   - Add to `.gitignore`
   - Purge from git history: `git filter-branch --tree-filter 'rm -rf obj bin .vs' HEAD`

3. **Secure configuration for DEVELOPMENT**
   - Use local `.env` files + environment variables
   - Never commit credentials
   - Use Azure Key Vault for shared/prod secrets

4. **Add HTTPS everywhere** (local dev with self-signed cert)

5. **Update .gitignore** (new repo or if upgrading)
   ```gitignore
   bin/
   obj/
   .vs/
   .vscode/settings.json
   .env
   .env.local
   appsettings.Development.json
   app.config
   Web.config
   ```

---

## PHASE 1 – NEW PROJECT STRUCTURE (Weeks 2–4)

### Architecture: Modular Monolith (ASP.NET Core 8)

Start with clean, modern structure. You can split services later.

```
InsureX/
├── src/
│   ├── InsureX.Web                  [ASP.NET Core 8 – Razor, React SPA host]
│   ├── InsureX.Api                  [ASP.NET Core 8 Web API – JSON endpoints]
│   ├── InsureX.Application          [Business services, rules, orchestration]
│   ├── InsureX.Domain               [Entities, enums, domain events, interfaces]
│   ├── InsureX.Infrastructure       [EF Core, SQL Server repos, integrations]
│   ├── InsureX.Shared               [DTOs, exceptions, constants, validators]
│   └── InsureX.Frontend             [React/TypeScript – separate SPA or embedded]
├── tests/
│   ├── InsureX.UnitTests
│   ├── InsureX.IntegrationTests
│   └── InsureX.Api.Tests
├── docker/
│   ├── Dockerfile.api
│   ├── Dockerfile.web
│   └── docker-compose.yml
├── sql/
│   ├── migrations/
│   ├── seed-data.sql
│   └── stored-procedures/           [if needed for reporting]
├── docs/
│   ├── API.md
│   ├── ARCHITECTURE.md
│   └── SETUP.md
└── .env.example
```

### Choice: React + TypeScript Frontend

You can deploy React in **two ways**:

**Option A (Recommended for B2B SaaS):** Embed React in ASP.NET Core
- Single deployment + CORS not needed
- Easier session/auth management
- Simpler dev workflow
- Pattern: Web project contains `/wwwroot/app/` (React build output)

**Option B:** Separate SPA + .NET API
- Full separation of concerns
- Independent deployments
- CORS required
- Useful if you want different teams per frontend

**For this guide, I recommend Option A.** Both work; adjust based on your team size.

---

## PHASE 1 – CREATING THE PROJECT

### Step 1: Create Solution
```bash
# In Visual Studio or via CLI
dotnet new sln -n InsureX
cd InsureX

# Create projects
dotnet new webapi -n InsureX.Api -f net8.0
dotnet new classlib -n InsureX.Application -f net8.0
dotnet new classlib -n InsureX.Domain -f net8.0
dotnet new classlib -n InsureX.Infrastructure -f net8.0
dotnet new classlib -n InsureX.Shared -f net8.0

# Optional: Create separate React project (or use Node.js separately)
# npx create-react-app InsureX.Web --template typescript

# Add to solution
dotnet sln InsureX.sln add src/**/*.csproj
dotnet sln InsureX.sln add tests/**/*.csproj

# Add project references
cd src/InsureX.Api
dotnet add reference ../InsureX.Application/InsureX.Application.csproj
dotnet add reference ../InsureX.Domain/InsureX.Domain.csproj
dotnet add reference ../InsureX.Infrastructure/InsureX.Infrastructure.csproj
dotnet add reference ../InsureX.Shared/InsureX.Shared.csproj
# ... repeat for Application, Infrastructure
```

### Step 2: Add NuGet Packages

**Core:**
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
```

**Infrastructure:**
```
dotnet add package Azure.Identity
dotnet add package Azure.Security.KeyVault.Secrets
dotnet add package Azure.Storage.Blobs
dotnet add package Azure.Messaging.ServiceBus
dotnet add package Microsoft.Data.SqlClient
dotnet add package Serilog
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.ApplicationInsights
dotnet add package Polly
dotnet add package Polly.CircuitBreaker
```

**Testing:**
```
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

---

## PHASE 2 – MULTI-TENANCY FOUNDATION (Weeks 5–8)

### 2.1 Core Tenant Model (Domain Layer)

**InsureX.Domain/Tenants/Tenant.cs**
```csharp
public class Tenant
{
    public Guid Id { get; set; }
    public string Code { get; set; }              // "bank-a", "insurer-xyz"
    public string Name { get; set; }
    public TenantType Type { get; set; }          // Bank, Insurer, Broker
    public TenantStatus Status { get; set; }      // Active, Inactive, Suspended
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }
}

public enum TenantType { Bank = 1, Insurer = 2, Broker = 3, Admin = 4 }
public enum TenantStatus { Active = 1, Inactive = 2, Suspended = 3 }
```

**InsureX.Domain/Tenants/ITenantContext.cs**
```csharp
public interface ITenantContext
{
    Guid TenantId { get; }
    Guid? UserId { get; }
    string? Username { get; }
    IEnumerable<string> Roles { get; }
    TenantType TenantType { get; }
    
    void SetTenant(Guid tenantId, string tenantCode, TenantType type);
}
```

**InsureX.Infrastructure/Tenants/TenantContext.cs**
```csharp
public class TenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Guid TenantId { get; private set; }
    public Guid? UserId { get; private set; }
    public string? Username { get; private set; }
    public IEnumerable<string> Roles { get; private set; } = new List<string>();
    public TenantType TenantType { get; private set; }

    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        ResolveTenant();
    }

    private void ResolveTenant()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        // Priority 1: JWT claim
        var tenantClaim = httpContext.User?.FindFirst("tenant_id");
        if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
        {
            TenantId = tenantId;
            UserId = httpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) is string uid 
                ? Guid.Parse(uid) 
                : null;
            Username = httpContext.User?.FindFirstValue(ClaimTypes.Name);
            Roles = httpContext.User?.FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
            
            return;
        }

        // Priority 2: Subdomain
        var host = httpContext.Request.Host.Host;
        var parts = host.Split('.');
        if (parts.Length > 0 && parts[0] != "localhost")
        {
            // tenant.insurex.local
            // Use database lookup to find tenant by code
            // For now, derive from subdomain
            var tenantCode = parts[0];
            // Store in TenantId (you'll populate this from a service)
        }
    }

    public void SetTenant(Guid tenantId, string tenantCode, TenantType type)
    {
        TenantId = tenantId;
        TenantType = type;
    }
}
```

### 2.2 EF Core Multi-Tenancy Setup

**InsureX.Infrastructure/Persistence/ApplicationDbContext.cs**
```csharp
public class ApplicationDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    // Tenant & Identity
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    // Registry
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Policy> Policies { get; set; }
    public DbSet<Borrower> Borrowers { get; set; }

    // Compliance
    public DbSet<ComplianceRuleSet> ComplianceRuleSets { get; set; }
    public DbSet<ComplianceState> ComplianceStates { get; set; }
    public DbSet<ComplianceDecision> ComplianceDecisions { get; set; }

    // Workflow
    public DbSet<NonComplianceCase> Cases { get; set; }
    public DbSet<CaseTask> Tasks { get; set; }
    public DbSet<SLA> SLAs { get; set; }

    // Audit
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<EventEnvelope> EventEnvelopes { get; set; }
    
    // Outbox (for event publishing)
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply tenant filters globally
        var tenantEntities = modelBuilder.Model
            .GetEntityTypes()
            .Where(t => typeof(ITenantScoped).IsAssignableFrom(t.ClrType));

        foreach (var entityType in tenantEntities)
        {
            var parameter = Expression.Parameter(entityType.ClrType, "p");
            var property = Expression.Property(parameter, nameof(ITenantScoped.TenantId));
            var tenantIdValue = Expression.Constant(_tenantContext.TenantId);
            var body = Expression.Equal(property, tenantIdValue);
            var lambda = Expression.Lambda(body, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }

        // Configure indexes
        modelBuilder.Entity<Asset>()
            .HasIndex(a => new { a.TenantId, a.CreatedUtc })
            .HasDatabaseName("IX_Assets_TenantId_CreatedUtc");

        modelBuilder.Entity<Asset>()
            .HasIndex(a => new { a.TenantId, a.Status })
            .HasDatabaseName("IX_Assets_TenantId_Status");

        // ... configure other entities
    }
}

public interface ITenantScoped
{
    Guid TenantId { get; }
}
```

### 2.3 SQL Server Row-Level Security (RLS) – Optional but Recommended

This provides a **bank-grade safety net** if a dev forgets a tenant filter:

**SQL Setup (run in SSMS):**
```sql
-- Create security predicate function
CREATE FUNCTION dbo.fn_TenantPredicate(@TenantId UNIQUEIDENTIFIER)
RETURNS TABLE
WITH SCHEMABINDING
AS RETURN
SELECT 1 as result
WHERE CAST(SESSION_CONTEXT(N'TenantId') AS UNIQUEIDENTIFIER) = @TenantId;

-- Apply to Assets table (example)
CREATE SECURITY POLICY rls_TenantPolicy
    ADD FILTER PREDICATE dbo.fn_TenantPredicate(TenantId) ON dbo.Assets,
    ADD FILTER PREDICATE dbo.fn_TenantPredicate(TenantId) ON dbo.Policies,
    ADD FILTER PREDICATE dbo.fn_TenantPredicate(TenantId) ON dbo.Cases
WITH (STATE = ON, SCHEMABINDING = ON);

-- EF Core interceptor to set SESSION_CONTEXT on each connection
```

**EF Core interceptor (Infrastructure):**
```csharp
public class TenantContextInterceptor : DbConnectionInterceptor
{
    private readonly ITenantContext _tenantContext;

    public TenantContextInterceptor(ITenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public override async Task<InterceptionResult> ConnectionOpeningAsync(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
    {
        var dbConnection = connection as SqlConnection;
        dbConnection?.StatementCompleted += (s, e) => { /* logging */ };

        return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
    }

    public override InterceptionResult ConnectionOpened(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result)
    {
        SetSessionContext(connection);
        return base.ConnectionOpened(connection, eventData, result);
    }

    private void SetSessionContext(DbConnection connection)
    {
        if (_tenantContext.TenantId == Guid.Empty) return;

        if (connection is SqlConnection sqlConnection)
        {
            using (var cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = $"EXEC sp_set_session_context @key=N'TenantId', @value=N'{_tenantContext.TenantId}'";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
```

### 2.4 Register Services (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
        .AddInterceptors(new TenantContextInterceptor(
            builder.Services.BuildServiceProvider().GetRequiredService<ITenantContext>())));

// Tenancy
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddHttpContextAccessor();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "your-auth-server";
        options.Audience = "insurex-api";
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

// Logging
builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.ApplicationInsights(
            new TelemetryClient(),
            TelemetryConverter.Traces));

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// EF Core migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
```

---

## PHASE 3 – AUTHENTICATION & AUTHORIZATION (Weeks 9–10)

### 3.1 User & Role Model

**InsureX.Domain/Identity/User.cs**
```csharp
public class User : IdentityUser<Guid>, ITenantScoped
{
    public Guid TenantId { get; set; }
    public Guid OrganisationId { get; set; }
    public UserStatus Status { get; set; }
    public DateTime LastLoginUtc { get; set; }
    public DateTime CreatedUtc { get; set; }
}

public enum UserStatus { Active = 1, Inactive = 2, Suspended = 3 }
```

**InsureX.Domain/Identity/Role.cs**
```csharp
public class Role : IdentityRole<Guid>, ITenantScoped
{
    public Guid TenantId { get; set; }
    public string Description { get; set; }
    public List<Permission> Permissions { get; set; } = new();
}

public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; }              // "assets:read", "cases:escalate"
    public string Description { get; set; }
}
```

### 3.2 Login Endpoint (API Controller)

**InsureX.Api/Controllers/AuthController.cs**
```csharp
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null || user.Status == UserStatus.Suspended)
            return Unauthorized(new { message = "Invalid credentials or user suspended" });

        var result = await _signInManager.CheckPasswordSignInAsync(
            user, request.Password, lockoutOnFailure: true);
        
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid credentials" });

        var token = await _tokenService.GenerateTokenAsync(user);
        user.LastLoginUtc = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return Ok(new { token, expiresIn = 3600 });
    }
}
```

### 3.3 JWT Token Service

**InsureX.Application/Services/TokenService.cs**
```csharp
public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;

    public async Task<string> GenerateTokenAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new("tenant_id", user.TenantId.ToString()),
            new("org_id", user.OrganisationId.ToString()),
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

---

## PHASE 4 – CORE DOMAIN MODELS (Weeks 11–14)

### 4.1 Asset & Finance Registry

**InsureX.Domain/Registry/Asset.cs**
```csharp
public class Asset : IAggregateRoot, ITenantScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid BankOrganisationId { get; set; }
    public string AssetType { get; set; }                    // "Motor", "NonMotor"
    public string AssetIdentifier { get; set; }             // VIN, serial, IMEI
    public string? RegistrationNumber { get; set; }
    public decimal FinancedAmount { get; set; }
    public string? BorrowerReference { get; set; }
    public Guid? BorrowerId { get; set; }
    public DateTime LoanStartDate { get; set; }
    public DateTime LoanEndDate { get; set; }
    public AssetStatus Status { get; set; }                 // Active, Settled, Closed
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }

    // Navigation
    public List<Policy> Policies { get; set; } = new();
    public ComplianceState? ComplianceState { get; set; }
}

public enum AssetStatus { Active = 1, Settled = 2, Closed = 3 }
```

**InsureX.Domain/Registry/Policy.cs**
```csharp
public class Policy : ITenantScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? AssetId { get; set; }
    public Guid InsurerOrganisationId { get; set; }
    public string PolicyNumber { get; set; }
    public string ProductType { get; set; }                 // "Comprehensive", "Third-Party"
    public PolicyStatus Status { get; set; }                // Active, Lapsed, Cancelled
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal InsuredValue { get; set; }
    public decimal PremiumAmount { get; set; }
    public PremiumPaymentStatus PaymentStatus { get; set; }  // Paid, Overdue, Arrears
    public DateTime? LastPremiumPaymentDate { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }

    // Navigation
    public Asset? Asset { get; set; }
}

public enum PolicyStatus { Active = 1, Lapsed = 2, Cancelled = 3 }
public enum PremiumPaymentStatus { Paid = 1, Overdue = 2, Arrears = 3 }
```

### 4.2 Compliance Model

**InsureX.Domain/Compliance/ComplianceState.cs**
```csharp
public class ComplianceState : ITenantScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid AssetId { get; set; }
    public ComplianceStatus Status { get; set; }            // Compliant, NonCompliant, Pending, Unknown
    public string? NonComplianceReason { get; set; }
    public DateTime? LastEvaluatedUtc { get; set; }
    public DateTime? LastChangedUtc { get; set; }
    public Guid? ActiveCaseId { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }

    // Navigation
    public Asset Asset { get; set; }
    public NonComplianceCase? ActiveCase { get; set; }
}

public enum ComplianceStatus { Compliant = 1, NonCompliant = 2, Pending = 3, Unknown = 4 }
```

**InsureX.Domain/Compliance/ComplianceDecision.cs**
```csharp
public class ComplianceDecision : ITenantScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid AssetId { get; set; }
    public ComplianceStatus OldStatus { get; set; }
    public ComplianceStatus NewStatus { get; set; }
    public string DecisionReason { get; set; }
    public string? RuleSetVersion { get; set; }
    public DateTime CreatedUtc { get; set; }
}
```

### 4.3 Workflow & Case Management

**InsureX.Domain/Workflow/NonComplianceCase.cs**
```csharp
public class NonComplianceCase : IAggregateRoot, ITenantScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid AssetId { get; set; }
    public Guid ComplianceStateId { get; set; }
    public string CaseNumber { get; set; }
    public string NonComplianceReason { get; set; }         // "PremiumOverdue", "PolicyCancelled"
    public CaseStatus Status { get; set; }                  // Open, InProgress, Escalated, Resolved, Closed
    public CaseSeverity Severity { get; set; }              // Low, Medium, High
    public Guid? AssignedToUserId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }

    // Navigation
    public List<CaseTask> Tasks { get; set; } = new();
    public List<CaseEvent> Events { get; set; } = new();
}

public class CaseTask : ITenantScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid CaseId { get; set; }
    public string TaskType { get; set; }                    // "SendSms", "SendEmail", "CreateTicket", "Call"
    public TaskStatus Status { get; set; }                  // Pending, InProgress, Completed, Failed
    public string? Description { get; set; }
    public int RetryCount { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }
}

public enum CaseStatus { Open = 1, InProgress = 2, Escalated = 3, Resolved = 4, Closed = 5 }
public enum CaseSeverity { Low = 1, Medium = 2, High = 3 }
public enum TaskStatus { Pending = 1, InProgress = 2, Completed = 3, Failed = 4 }
```

### 4.4 Audit & Evidence

**InsureX.Domain/Audit/AuditLog.cs**
```csharp
public class AuditLog : ITenantScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Action { get; set; }                      // "AssetCreated", "ComplianceDecision"
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public Guid? ActorUserId { get; set; }
    public string? ActorSystem { get; set; }                // "InsurerConnector_AIG", "BankFeed_ACB"
    public string? OldValues { get; set; }                  // JSON
    public string? NewValues { get; set; }                  // JSON
    public string? CorrelationId { get; set; }
    public DateTime CreatedUtc { get; set; }
}

public class EventEnvelope : ITenantScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string EventType { get; set; }                  // "PolicyStatusChanged", "PremiumPaymentFailed"
    public string SourceSystem { get; set; }                // "Insurer_AIG", "Bank_Acme"
    public string SourceEventId { get; set; }               // Deduplication
    public string RawPayload { get; set; }                  // Original JSON/XML (stored in Blob)
    public string? NormalizedPayload { get; set; }          // Canonical format (JSON)
    public ProcessingStatus Status { get; set; }            // Received, Normalized, ProcessedDecision, Closed
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }
}

public enum ProcessingStatus { Received = 1, Normalized = 2, ProcessedDecision = 3, Closed = 4 }
```

### 4.5 Outbox Pattern (for reliable event publishing)

**InsureX.Domain/Outbox/OutboxMessage.cs**
```csharp
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string AggregateId { get; set; }
    public string EventType { get; set; }
    public string Payload { get; set; }                      // JSON
    public string? Topic { get; set; }                       // Service Bus topic
    public OutboxStatus Status { get; set; }                // Pending, Published, Failed
    public DateTime CreatedUtc { get; set; }
    public DateTime? PublishedUtc { get; set; }
}

public enum OutboxStatus { Pending = 1, Published = 2, Failed = 3 }
```

---

## PHASE 5 – APPLICATION SERVICES & BUSINESS LOGIC (Weeks 15–18)

### 5.1 Compliance Rules Engine

**InsureX.Application/Compliance/ComplianceRuleSet.cs**
```csharp
public class ComplianceRuleSet
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }                        // "Default Bank Rules v1"
    public string Version { get; set; }
    public List<ComplianceRule> Rules { get; set; } = new();
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}

public class ComplianceRule
{
    public Guid Id { get; set; }
    public string Code { get; set; }                        // "PREMIUM_OVERDUE_30D"
    public string Name { get; set; }
    public int Days { get; set; }                           // X days for grace period
    public RuleSeverity Severity { get; set; }              // Low, Medium, High
    public bool IsActive { get; set; }
}

public enum RuleSeverity { Low = 1, Medium = 2, High = 3 }
```

**InsureX.Application/Compliance/ComplianceEvaluationService.cs**
```csharp
public interface IComplianceEvaluationService
{
    Task<ComplianceDecision?> EvaluateAssetAsync(Guid assetId);
}

public class ComplianceEvaluationService : IComplianceEvaluationService
{
    private readonly IAssetRepository _assetRepository;
    private readonly IPolicyRepository _policyRepository;
    private readonly IComplianceStateRepository _stateRepository;
    private readonly IComplianceRuleRepository _ruleRepository;
    private readonly ITenantContext _tenantContext;

    public async Task<ComplianceDecision?> EvaluateAssetAsync(Guid assetId)
    {
        var asset = await _assetRepository.GetByIdAsync(assetId);
        if (asset == null) return null;

        // Fetch active policies for this asset
        var policies = await _policyRepository.GetActiveByAssetAsync(assetId);
        
        // Get applicable rules
        var ruleSet = await _ruleRepository.GetActiveAsync(_tenantContext.TenantId);
        
        // Evaluate
        var newStatus = DetermineComplianceStatus(asset, policies, ruleSet);
        
        // Get current state
        var currentState = await _stateRepository.GetByAssetIdAsync(assetId);
        
        // If status changed, create decision
        if (currentState?.Status != newStatus)
        {
            var decision = new ComplianceDecision
            {
                Id = Guid.NewGuid(),
                TenantId = _tenantContext.TenantId,
                AssetId = assetId,
                OldStatus = currentState?.Status ?? ComplianceStatus.Unknown,
                NewStatus = newStatus,
                DecisionReason = GetDecisionReason(asset, policies, ruleSet),
                RuleSetVersion = ruleSet?.Version,
                CreatedUtc = DateTime.UtcNow
            };

            // Update state
            if (currentState != null)
            {
                currentState.Status = newStatus;
                currentState.LastChangedUtc = DateTime.UtcNow;
                await _stateRepository.UpdateAsync(currentState);
            }
            else
            {
                var newState = new ComplianceState
                {
                    Id = Guid.NewGuid(),
                    TenantId = _tenantContext.TenantId,
                    AssetId = assetId,
                    Status = newStatus,
                    LastEvaluatedUtc = DateTime.UtcNow,
                    LastChangedUtc = DateTime.UtcNow,
                    CreatedUtc = DateTime.UtcNow
                };
                await _stateRepository.AddAsync(newState);
            }

            return decision;
        }

        return null;
    }

    private ComplianceStatus DetermineComplianceStatus(
        Asset asset,
        List<Policy> policies,
        ComplianceRuleSet ruleSet)
    {
        // No policies = non-compliant
        if (!policies.Any())
            return ComplianceStatus.NonCompliant;

        // Any policy not active = non-compliant
        if (policies.All(p => p.Status != PolicyStatus.Active))
            return ComplianceStatus.NonCompliant;

        // Any policy with payment overdue > grace period = non-compliant
        var overdueRule = ruleSet.Rules.FirstOrDefault(r => r.Code == "PREMIUM_OVERDUE_30D");
        if (overdueRule != null)
        {
            var overduePolicies = policies.Where(p =>
                p.PaymentStatus == PremiumPaymentStatus.Overdue &&
                p.LastPremiumPaymentDate.HasValue &&
                (DateTime.UtcNow - p.LastPremiumPaymentDate.Value).TotalDays > overdueRule.Days);

            if (overduePolicies.Any())
                return ComplianceStatus.NonCompliant;
        }

        return ComplianceStatus.Compliant;
    }

    private string GetDecisionReason(Asset asset, List<Policy> policies, ComplianceRuleSet ruleSet)
    {
        if (!policies.Any())
            return "No active policies found";

        var inactivePolicies = policies.Where(p => p.Status != PolicyStatus.Active).ToList();
        if (inactivePolicies.Any())
            return $"Inactive policies: {string.Join(", ", inactivePolicies.Select(p => p.PolicyNumber))}";

        return "Compliant with all rules";
    }
}
```

### 5.2 Workflow Orchestration Service

**InsureX.Application/Workflow/WorkflowOrchestrationService.cs**
```csharp
public interface IWorkflowOrchestrationService
{
    Task CreateNonComplianceCaseAsync(
        Guid assetId,
        ComplianceDecision decision);
    
    Task EscalateCaseAsync(Guid caseId, string reason);
}

public class WorkflowOrchestrationService : IWorkflowOrchestrationService
{
    private readonly ICaseRepository _caseRepository;
    private readonly INotificationService _notificationService;
    private readonly ITenantContext _tenantContext;

    public async Task CreateNonComplianceCaseAsync(
        Guid assetId,
        ComplianceDecision decision)
    {
        // Check if case already exists
        var existingCase = await _caseRepository.GetActiveByAssetAsync(assetId);
        if (existingCase != null)
            return;

        var caseNumber = GenerateCaseNumber();
        var newCase = new NonComplianceCase
        {
            Id = Guid.NewGuid(),
            TenantId = _tenantContext.TenantId,
            AssetId = assetId,
            CaseNumber = caseNumber,
            NonComplianceReason = decision.DecisionReason,
            Status = CaseStatus.Open,
            Severity = GetSeverity(decision),
            CreatedUtc = DateTime.UtcNow,
            ModifiedUtc = DateTime.UtcNow
        };

        await _caseRepository.AddAsync(newCase);

        // Create initial tasks
        var tasks = new List<CaseTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                TenantId = _tenantContext.TenantId,
                CaseId = newCase.Id,
                TaskType = "SendEmail",
                Status = TaskStatus.Pending,
                Description = $"Send compliance notice email for {newCase.CaseNumber}"
            },
            new()
            {
                Id = Guid.NewGuid(),
                TenantId = _tenantContext.TenantId,
                CaseId = newCase.Id,
                TaskType = "SendSms",
                Status = TaskStatus.Pending,
                Description = $"Send compliance notice SMS for {newCase.CaseNumber}"
            }
        };

        foreach (var task in tasks)
        {
            await _caseRepository.AddTaskAsync(task);
        }

        // Publish events for notification service
        // (via outbox pattern)
    }

    private CaseSeverity GetSeverity(ComplianceDecision decision)
    {
        return decision.NewStatus == ComplianceStatus.NonCompliant 
            ? CaseSeverity.High 
            : CaseSeverity.Medium;
    }

    private string GenerateCaseNumber()
    {
        return $"CASE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    public async Task EscalateCaseAsync(Guid caseId, string reason)
    {
        var caseEntity = await _caseRepository.GetByIdAsync(caseId);
        if (caseEntity == null)
            throw new InvalidOperationException($"Case {caseId} not found");

        caseEntity.Status = CaseStatus.Escalated;
        caseEntity.ModifiedUtc = DateTime.UtcNow;

        await _caseRepository.UpdateAsync(caseEntity);

        // Create escalation task + audit log
    }
}
```

---

## PHASE 6 – REST API ENDPOINTS (Weeks 19–22)

### 6.1 Asset Management API

**InsureX.Api/Controllers/AssetsController.cs**
```csharp
[ApiController]
[Route("api/v1/assets")]
[Authorize]
public class AssetsController : ControllerBase
{
    private readonly IAssetService _assetService;
    private readonly ITenantContext _tenantContext;

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<AssetDto>>> GetAssets(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? status = null,
        [FromQuery] string? assetType = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        if (pageSize > 100) pageSize = 100;
        if (page < 1) page = 1;

        var (assets, total) = await _assetService.GetAssetsAsync(
            page, pageSize, status, assetType, fromDate, toDate);

        var response = new PaginatedResponse<AssetDto>
        {
            Items = assets.Select(AssetDto.FromDomain).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = total,
            TotalPages = (total + pageSize - 1) / pageSize,
            HasNext = page < (total + pageSize - 1) / pageSize
        };

        Response.Headers.Add("X-Correlation-Id", HttpContext.TraceIdentifier);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetDto>> GetAsset(Guid id)
    {
        var asset = await _assetService.GetAssetByIdAsync(id);
        if (asset == null)
            return NotFound(new ProblemDetails { Title = "Asset not found" });

        return Ok(AssetDto.FromDomain(asset));
    }

    [HttpPost]
    [Authorize(Roles = "BankUser,Admin")]
    public async Task<ActionResult<AssetDto>> CreateAsset([FromBody] CreateAssetRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var asset = await _assetService.CreateAssetAsync(request);
        return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, AssetDto.FromDomain(asset));
    }

    [HttpPost("import")]
    [Authorize(Roles = "BankUser,Admin")]
    public async Task<ActionResult<ImportJobDto>> ImportAssets([FromBody] ImportAssetsRequest request)
    {
        // Async import job
        var jobId = await _assetService.StartImportAsync(request);
        return Accepted(new { jobId, statusUrl = $"/api/v1/import-jobs/{jobId}" });
    }
}
```

### 6.2 Compliance API

**InsureX.Api/Controllers/ComplianceController.cs**
```csharp
[ApiController]
[Route("api/v1/compliance")]
[Authorize]
public class ComplianceController : ControllerBase
{
    private readonly IComplianceService _complianceService;

    [HttpGet("assets")]
    public async Task<ActionResult<PaginatedResponse<ComplianceStateDto>>> GetComplianceStates(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? status = null,
        [FromQuery] DateTime? fromDate = null)
    {
        if (pageSize > 100) pageSize = 100;

        var (states, total) = await _complianceService.GetStatesAsync(page, pageSize, status, fromDate);

        return Ok(new PaginatedResponse<ComplianceStateDto>
        {
            Items = states.Select(ComplianceStateDto.FromDomain).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = total,
            TotalPages = (total + pageSize - 1) / pageSize,
            HasNext = page < (total + pageSize - 1) / pageSize
        });
    }

    [HttpGet("assets/{assetId}")]
    public async Task<ActionResult<ComplianceDetailDto>> GetAssetCompliance(Guid assetId)
    {
        var detail = await _complianceService.GetAssetComplianceDetailAsync(assetId);
        if (detail == null)
            return NotFound();

        return Ok(detail);
    }

    [HttpGet("assets/{assetId}/decisions")]
    public async Task<ActionResult<PaginatedResponse<ComplianceDecisionDto>>> GetDecisionHistory(
        Guid assetId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var (decisions, total) = await _complianceService.GetDecisionHistoryAsync(assetId, page, pageSize);

        return Ok(new PaginatedResponse<ComplianceDecisionDto>
        {
            Items = decisions.Select(ComplianceDecisionDto.FromDomain).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = total,
            TotalPages = (total + pageSize - 1) / pageSize,
            HasNext = page < (total + pageSize - 1) / pageSize
        });
    }
}
```

### 6.3 Cases / Workflow API

**InsureX.Api/Controllers/CasesController.cs**
```csharp
[ApiController]
[Route("api/v1/cases")]
[Authorize]
public class CasesController : ControllerBase
{
    private readonly ICaseService _caseService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<CaseDto>>> GetCases(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? status = null,
        [FromQuery] string? severity = null)
    {
        var (cases, total) = await _caseService.GetCasesAsync(page, pageSize, status, severity);

        return Ok(new PaginatedResponse<CaseDto>
        {
            Items = cases.Select(CaseDto.FromDomain).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = total,
            TotalPages = (total + pageSize - 1) / pageSize,
            HasNext = page < (total + pageSize - 1) / pageSize
        });
    }

    [HttpPost("{caseId}/actions/escalate")]
    [Authorize(Roles = "BankUser,InsurerUser,Admin")]
    public async Task<IActionResult> EscalateCase(Guid caseId, [FromBody] EscalateCaseRequest request)
    {
        await _caseService.EscalateCaseAsync(caseId, request.Reason);
        return NoContent();
    }

    [HttpPost("{caseId}/actions/close")]
    [Authorize(Roles = "BankUser,InsurerUser,Admin")]
    public async Task<IActionResult> CloseCase(Guid caseId, [FromBody] CloseCaseRequest request)
    {
        await _caseService.CloseCaseAsync(caseId, request.ResolutionNotes);
        return NoContent();
    }
}
```

### 6.4 Insurer Webhook API (Security Critical)

**InsureX.Api/Controllers/IntegrationController.cs**
```csharp
[ApiController]
[Route("api/v1/integrations")]
public class IntegrationController : ControllerBase
{
    private readonly IInsurerWebhookService _webhookService;
    private readonly IHmacValidator _hmacValidator;
    private readonly ITenantContext _tenantContext;

    [HttpPost("insurers/{insurerCode}/webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> ReceiveInsurerWebhook(
        string insurerCode,
        [FromBody] object payload,
        [FromHeader(Name = "X-Webhook-Signature")] string signature,
        [FromHeader(Name = "X-Webhook-Timestamp")] string timestamp,
        [FromHeader(Name = "X-Webhook-Nonce")] string nonce,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
    {
        // Validate signature
        if (!_hmacValidator.IsValidSignature(payload, signature, timestamp, nonce, insurerCode))
            return Unauthorized(new ProblemDetails { Title = "Invalid signature" });

        // Validate timestamp (prevent replay attacks)
        if (!IsRecentTimestamp(timestamp, allowedWindowSeconds: 300))
            return BadRequest(new ProblemDetails { Title = "Request expired" });

        // Deduplicate by idempotency key
        var isDuplicate = await _webhookService.HasBeenProcessedAsync(insurerCode, idempotencyKey);
        if (isDuplicate)
            return Ok(new { message = "Already processed" });

        // Process webhook
        try
        {
            await _webhookService.ProcessWebhookAsync(insurerCode, payload, idempotencyKey);
            return Ok(new { message = "Accepted" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ProblemDetails { Title = ex.Message });
        }
    }

    private bool IsRecentTimestamp(string timestamp, int allowedWindowSeconds)
    {
        if (!long.TryParse(timestamp, out var ts))
            return false;

        var requestTime = DateTimeOffset.FromUnixTimeSeconds(ts);
        var now = DateTimeOffset.UtcNow;
        var diff = (now - requestTime).TotalSeconds;

        return Math.Abs(diff) <= allowedWindowSeconds;
    }
}
```

---

## PHASE 7 – REACT FRONTEND (Weeks 23–26)

### 7.1 React Project Setup

```bash
# Option A: Create within ASP.NET Core project
cd InsureX.Web/wwwroot
npx create-react-app app --template typescript

# Or Option B: Separate SPA (then configure CORS)
npx create-react-app InsureX.Frontend --template typescript
```

### 7.2 React TypeScript Structure

```
src/
├── api/
│   ├── clients/
│   │   ├── assetClient.ts
│   │   ├── complianceClient.ts
│   │   ├── caseClient.ts
│   │   └── authClient.ts
│   └── types/
│       ├── Asset.ts
│       ├── Compliance.ts
│       ├── Case.ts
│       └── Common.ts
├── components/
│   ├── layouts/
│   │   └── MainLayout.tsx
│   ├── pages/
│   │   ├── AssetsPage.tsx
│   │   ├── CompliancePage.tsx
│   │   ├── CasesPage.tsx
│   │   └── LoginPage.tsx
│   ├── shared/
│   │   ├── Table.tsx
│   │   ├── Pagination.tsx
│   │   ├── StatusBadge.tsx
│   │   └── LoadingSpinner.tsx
│   └── forms/
│       ├── AssetForm.tsx
│       └── FilterForm.tsx
├── hooks/
│   ├── useAuth.ts
│   ├── useTenant.ts
│   └── usePagination.ts
├── context/
│   ├── AuthContext.tsx
│   └── TenantContext.tsx
├── utils/
│   ├── api.ts            (Axios instance + interceptors)
│   ├── formatters.ts
│   └── validators.ts
└── App.tsx
```

### 7.3 API Client Example (TypeScript)

**src/api/types/Common.ts**
```typescript
export interface PaginatedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
}

export interface Asset {
  id: string;
  assetType: string;
  assetIdentifier: string;
  financedAmount: number;
  status: 'Active' | 'Settled' | 'Closed';
  createdUtc: string;
}

export interface ComplianceState {
  id: string;
  assetId: string;
  status: 'Compliant' | 'NonCompliant' | 'Pending' | 'Unknown';
  nonComplianceReason: string | null;
  lastEvaluatedUtc: string | null;
}
```

**src/api/clients/assetClient.ts**
```typescript
import { API } from '../utils/api';
import { Asset, PaginatedResponse } from '../types/Common';

export const assetClient = {
  getAssets: async (page: number = 1, pageSize: number = 25) => {
    const response = await API.get<PaginatedResponse<Asset>>('/assets', {
      params: { page, pageSize }
    });
    return response.data;
  },

  getAsset: async (id: string) => {
    const response = await API.get<Asset>(`/assets/${id}`);
    return response.data;
  },

  createAsset: async (data: Partial<Asset>) => {
    const response = await API.post<Asset>('/assets', data);
    return response.data;
  }
};
```

**src/utils/api.ts**
```typescript
import axios from 'axios';
import { useAuthStore } from '../stores/auth';

export const API = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'http://localhost:5000/api/v1',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
});

API.interceptors.request.use(config => {
  const token = useAuthStore.getState().token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  config.headers['X-Correlation-Id'] = generateCorrelationId();
  return config;
});

API.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      // Redirect to login
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

function generateCorrelationId(): string {
  return `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
}
```

### 7.4 React Component Examples

**src/components/pages/AssetsPage.tsx**
```typescript
import React, { useState, useEffect } from 'react';
import { assetClient } from '../../api/clients/assetClient';
import { Asset, PaginatedResponse } from '../../api/types/Common';
import Table from '../shared/Table';
import Pagination from '../shared/Pagination';
import FilterForm from '../forms/FilterForm';
import LoadingSpinner from '../shared/LoadingSpinner';

export const AssetsPage: React.FC = () => {
  const [assets, setAssets] = useState<Asset[]>([]);
  const [pagination, setPagination] = useState({ page: 1, pageSize: 25, totalPages: 1 });
  const [loading, setLoading] = useState(false);
  const [filters, setFilters] = useState({ status: '', assetType: '' });

  const loadAssets = async (page: number) => {
    setLoading(true);
    try {
      const response = await assetClient.getAssets(page, pagination.pageSize);
      setAssets(response.items);
      setPagination({
        page: response.page,
        pageSize: response.pageSize,
        totalPages: response.totalPages
      });
    } catch (error) {
      console.error('Failed to load assets', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAssets(1);
  }, []);

  const columns = [
    { key: 'assetIdentifier', label: 'Asset ID', width: '20%' },
    { key: 'assetType', label: 'Type', width: '15%' },
    { key: 'financedAmount', label: 'Financed Amount', width: '15%' },
    { key: 'status', label: 'Status', width: '15%' },
    { key: 'createdUtc', label: 'Created', width: '20%' }
  ];

  return (
    <div className="assets-page">
      <h1>Assets Register</h1>
      
      <FilterForm onApply={setFilters} />
      
      {loading ? (
        <LoadingSpinner />
      ) : (
        <>
          <Table columns={columns} data={assets} />
          <Pagination
            current={pagination.page}
            total={pagination.totalPages}
            onPageChange={loadAssets}
          />
        </>
      )}
    </div>
  );
};
```

---

## PHASE 8 – DATABASE SETUP (SQL SERVER) (Weeks 27–28)

### 8.1 Connection String Setup

**appsettings.json (local development)**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InsureXDb;Integrated Security=True;Encrypt=False;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### 8.2 Entity Framework Migrations

```bash
# Add EF Core tools
dotnet tool install --global dotnet-ef

# From InsureX.Infrastructure project
cd src/InsureX.Infrastructure

# Create initial migration
dotnet ef migrations add InitialCreate -p ../InsureX.Infrastructure -s ../../src/InsureX.Api -o Data/Migrations

# View migration script
dotnet ef migrations script InitialCreate

# Apply to database
dotnet ef database update
```

### 8.3 Seed Data (Optional)

**sql/seed-data.sql**
```sql
-- Insert test tenant
INSERT INTO dbo.Tenants (Id, Code, Name, Type, Status, CreatedUtc, ModifiedUtc)
VALUES (NEWID(), 'test-bank-1', 'Test Bank', 1, 1, GETUTCDATE(), GETUTCDATE());

-- Insert test users
INSERT INTO dbo.Users (Id, TenantId, OrganisationId, UserName, Email, Status, CreatedUtc)
VALUES (NEWID(), (SELECT Id FROM Tenants WHERE Code='test-bank-1'), NEWID(), 'testuser', 'test@bank.com', 1, GETUTCDATE());
```

---

## PHASE 9 – DEPLOYMENT & CONTAINERIZATION (Weeks 29–30)

### 9.1 Docker Setup

**Dockerfile.api**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src

COPY ["src/InsureX.Api/InsureX.Api.csproj", "src/InsureX.Api/"]
COPY ["src/InsureX.Application/InsureX.Application.csproj", "src/InsureX.Application/"]
COPY ["src/InsureX.Domain/InsureX.Domain.csproj", "src/InsureX.Domain/"]
COPY ["src/InsureX.Infrastructure/InsureX.Infrastructure.csproj", "src/InsureX.Infrastructure/"]
COPY ["src/InsureX.Shared/InsureX.Shared.csproj", "src/InsureX.Shared/"]

RUN dotnet restore "src/InsureX.Api/InsureX.Api.csproj"

COPY . .
WORKDIR "/src/src/InsureX.Api"
RUN dotnet build "InsureX.Api.csproj" -c Release -o /app/build

FROM builder AS publish
RUN dotnet publish "InsureX.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "InsureX.Api.dll"]
```

**docker-compose.yml**
```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: YourStrongPassword123!
      ACCEPT_EULA: Y
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

  api:
    build:
      context: .
      dockerfile: docker/Dockerfile.api
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__DefaultConnection: "Server=sqlserver;Database=InsureXDb;User Id=sa;Password=YourStrongPassword123!;Encrypt=False;"
    ports:
      - "5000:80"
    depends_on:
      - sqlserver

  web:
    build:
      context: .
      dockerfile: docker/Dockerfile.web
    ports:
      - "3000:3000"
    depends_on:
      - api

volumes:
  sqldata:
```

**Run locally:**
```bash
docker-compose up -d
```

---

## SUMMARY CHECKLIST

### Security
- [ ] Rotate all exposed credentials
- [ ] Remove bin/, obj/, .vs/ from repo
- [ ] Configure Key Vault for secrets
- [ ] Implement TLS/HTTPS everywhere
- [ ] Enable SQL Server RLS
- [ ] Implement JWT/OAuth2
- [ ] Add signed webhook validation

### Architecture
- [ ] Set up ASP.NET Core 8 solution with layered structure
- [ ] Implement multi-tenancy (TenantContext + EF filters)
- [ ] Design domain models (Asset, Policy, Compliance, Case, Audit)
- [ ] Create repositories + services
- [ ] Build compliance rules engine
- [ ] Implement workflow orchestration

### API
- [ ] REST endpoints (Assets, Policies, Compliance, Cases)
- [ ] Standardized paging + error handling
- [ ] Insurer webhook endpoints (signed)
- [ ] Integration endpoints (banks, insurers)

### Frontend
- [ ] React + TypeScript setup
- [ ] API client with interceptors
- [ ] Key pages (Assets, Compliance, Cases)
- [ ] Bootstrap styling + responsive design

### Testing
- [ ] Unit tests (services, rules)
- [ ] Integration tests (repositories, EF Core)
- [ ] API contract tests

### DevOps
- [ ] Docker + docker-compose
- [ ] GitHub Actions / Azure DevOps pipeline
- [ ] Database migrations automated
- [ ] Environment config management

---

## QUICK START (TL;DR)

1. **Create solution** (Phase 1):
   ```bash
   dotnet new sln -n InsureX
   dotnet new webapi -n InsureX.Api -f net8.0
   ```

2. **Set up multi-tenancy** (Phase 2):
   - Implement TenantContext middleware
   - Add ITenantScoped interface
   - Configure EF Core global filters

3. **Build domain models** (Phase 4):
   - Asset, Policy, Compliance, Case, Audit entities
   - Create migrations

4. **Implement services** (Phase 5):
   - ComplianceEvaluationService
   - WorkflowOrchestrationService
   - NotificationService

5. **Create REST API** (Phase 6):
   - AssetsController, ComplianceController, CasesController
   - Secured with JWT + tenant validation
   - Standardized pagination

6. **Build React frontend** (Phase 7):
   - TypeScript + Axios clients
   - Components for assets, compliance, cases
   - Authentication flow

7. **Deploy**:
   - Docker containers
   - SQL Server database
   - Azure App Service or on-premises IIS

---

## NEXT STEPS

1. **Review this architecture** with your team
2. **Start Phase 0** (security fixes) immediately
3. **Create new solution** following Phase 1 structure
4. **Gradually migrate business logic** from old codebase
5. **Test thoroughly** (unit + integration)
6. **Deploy to staging** before production

**Estimated timeline:** 24–32 weeks for full MVP
**Team size:** 2–3 backend, 1–2 frontend, 1 DevOps

---

**Document version:** 1.0  
**Date:** January 2026  
**Status:** Ready for Implementation
