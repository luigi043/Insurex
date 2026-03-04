-- ============================================================
-- InsureX Row-Level Security (RLS) Setup Script
-- Applies TenantId-based security policies to key tables.
-- Run ONCE after the EF Code-First schema has been created.
-- ============================================================

-- ----------------------------------------------------------------
-- STEP 1: Create a security schema to hold the predicate function
-- ----------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Security')
    EXEC('CREATE SCHEMA Security');
GO

-- ----------------------------------------------------------------
-- STEP 2: Create the predicate function that checks TenantId
-- The function returns 1 (allow) when the row's TenantId matches
-- the SESSION_CONTEXT variable set at application login time.
-- ----------------------------------------------------------------
IF OBJECT_ID('Security.fn_tenantAccessPredicate', 'IF') IS NOT NULL
    DROP FUNCTION Security.fn_tenantAccessPredicate;
GO

CREATE FUNCTION Security.fn_tenantAccessPredicate(@TenantId INT)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS fn_accessResult
    WHERE
        -- System-level admins (TenantId = 0 or NULL in context) can see all rows
        TRY_CAST(SESSION_CONTEXT(N'TenantId') AS INT) IS NULL
        OR
        @TenantId = TRY_CAST(SESSION_CONTEXT(N'TenantId') AS INT);
GO

-- ----------------------------------------------------------------
-- STEP 3: Apply Security Policies to identified tenant-scoped tables
-- These table names match the entity models with TenantId fields.
-- NOTE: The policy is created as FILTER + BLOCK to prevent both
--       unauthorized reads AND unauthorized writes.
-- ----------------------------------------------------------------

-- AspNetUsers (users are tenant-scoped)
IF EXISTS (SELECT 1 FROM sys.security_policies WHERE name = 'TenantUserPolicy')
    DROP SECURITY POLICY TenantUserPolicy;
GO
CREATE SECURITY POLICY TenantUserPolicy
    ADD FILTER PREDICATE Security.fn_tenantAccessPredicate(TenantId)   ON [dbo].[AspNetUsers],
    ADD BLOCK  PREDICATE Security.fn_tenantAccessPredicate(TenantId)   ON [dbo].[AspNetUsers] AFTER INSERT
WITH (STATE = ON, SCHEMABINDING = ON);
GO

-- ----------------------------------------------------------------
-- STEP 4: Create a helper stored procedure to set SESSION_CONTEXT
-- Call this from the application after a user authenticates.
-- ----------------------------------------------------------------
IF OBJECT_ID('dbo.sp_SetTenantContext', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SetTenantContext;
GO

CREATE PROCEDURE dbo.sp_SetTenantContext
    @TenantId INT
AS
BEGIN
    EXEC sp_set_session_context N'TenantId', @TenantId, @read_only = 0;
END
GO

PRINT 'RLS Security Policy applied successfully.';
