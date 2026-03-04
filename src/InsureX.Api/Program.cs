using InsureX.Api.Middleware;
using InsureX.Application.Services;
using InsureX.Infrastructure.Data;
using InsureX.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "InsureX API", Version = "v1",
        Description = "Multi-tenant B2B SaaS platform for insurance compliance monitoring." });
});

// ── CORS (allow React dev server) ─────────────────────────────────────────────
builder.Services.AddCors(opt => opt.AddPolicy("ReactDev", p =>
    p.WithOrigins("http://localhost:5173", "http://localhost:3000")
     .AllowAnyHeader()
     .AllowAnyMethod()));

// ── Tenant context (scoped — fresh per request) ───────────────────────────────
builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<ITenantContext>(sp => sp.GetRequiredService<TenantContext>());

// ── EF Core / SQL Server ──────────────────────────────────────────────────────
builder.Services.AddDbContext<InsureXDbContext>((sp, opt) =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection")!;
    opt.UseSqlServer(cs);
});
builder.Services.AddScoped<IInsureXDbContext>(sp => sp.GetRequiredService<InsureXDbContext>());


// ── Application services ──────────────────────────────────────────────────────
builder.Services.AddScoped<AssetService>();
builder.Services.AddScoped<ComplianceService>();
builder.Services.AddScoped<PolicyService>();
builder.Services.AddScoped<CaseService>();

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Auto-migrate on startup (dev convenience) ─────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InsureXDbContext>();
    db.Database.Migrate();
}

// ── Pipeline ──────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactDev");
app.UseMiddleware<TenantContextMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
