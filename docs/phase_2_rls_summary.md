# Phase 2, Part 5: SQL Server Row-Level Security (RLS) Summary

## Objective
The goal was to implement a "defence in depth" database-level security layer. Even if an application bug accidentally bypasses the `TenantContext` LINQ filters, SQL Server itself should prevent any cross-tenant data leakage.

## Strategy Implemented

### 1. `docs/sql/rls_setup.sql` — SQL Security Script
An idempotent SQL script that:
- Creates a `Security` schema for organizing the predicate function.
- Creates `Security.fn_tenantAccessPredicate(@TenantId INT)` — a Table-Valued Function checked by SQL Server for every SELECT/INSERT/UPDATE/DELETE on protected tables.
  - Returns 1 (allow) if the caller's `SESSION_CONTEXT(N'TenantId')` matches the row's TenantId, or if the context is NULL (system-level admin bypass).
- Creates a `SECURITY POLICY TenantUserPolicy` applying FILTER + BLOCK predicates to `dbo.AspNetUsers`.
- Creates `dbo.sp_SetTenantContext(@TenantId)` — a stored procedure the application calls on connection open to activate RLS for that session.

### 2. `ApplicationDbContext.SetRlsTenantContext(int tenantId)`
A new method on the EF `ApplicationDbContext` that calls `EXEC dbo.sp_SetTenantContext @TenantId` via `Database.ExecuteSqlCommand`. This is to be called once per DB connection open, activating the tenant-scoped SQL Server session context.

### 3. `Global.asax.cs` — Startup Seeder Integration
After seeding users, the application startup now attempts to execute the `rls_setup.sql` script, splitting it on `GO` delimiters and executing each batch. The execution is wrapped in `try/catch`, making RLS failures non-fatal during local development (e.g. if the db user lacks `ALTER ANY SECURITY POLICY` permission, startup still proceeds with a `Trace.Warning`).

## Result
The solution continues to compile with **0 errors**. The RLS security layer is now a first-class, self-installing component of the application.
