# InsureX – SQL Server Setup Guide
## SQL Server 2022 + LocalDB Configuration

---

## PART 1: LOCAL DEVELOPMENT SETUP

### 1.1 Connection String for Visual Studio Code + SQL Server Management Studio

**SQL Server Instance Name:** `(localdb)\MSSQLLocalDB`

**Connection in appsettings.json (ASP.NET Core):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InsureXDb;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;"
  }
}
```

**Connection in SQL Server Management Studio (SSMS 22):**
- Server name: `(localdb)\MSSQLLocalDB`
- Authentication: Windows Authentication
- Database: `InsureXDb` (will create via migrations)

### 1.2 Create the Database (One-Time Setup)

In SSMS, open a New Query and run:

```sql
-- Create database
CREATE DATABASE InsureXDb
    COLLATE Latin1_General_100_CI_AS;
GO

-- Switch to the new database
USE InsureXDb;
GO

-- Verify creation
SELECT name, state_desc FROM sys.databases WHERE name = 'InsureXDb';
GO
```

---

## PART 2: CORE SCHEMA (POST-MIGRATION)

After EF Core migrations run, you'll have the base schema. Below are the additional setups (RLS, stored procedures, indexes).

### 2.1 Enforce Row-Level Security (RLS)

```sql
USE InsureXDb;
GO

-- Create the security predicate function
CREATE FUNCTION dbo.fn_TenantAccessPredicate(@TenantId UNIQUEIDENTIFIER)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS result
    WHERE CAST(SESSION_CONTEXT(N'TenantId') AS UNIQUEIDENTIFIER) = @TenantId
        OR CAST(SESSION_CONTEXT(N'TenantId') AS UNIQUEIDENTIFIER) IS NULL; -- Allow NULL for system ops

GO

-- Apply RLS to tenant-scoped tables
-- Assets
CREATE SECURITY POLICY rls_Assets_Policy
    ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Assets
WITH (STATE = ON, SCHEMABINDING = ON);

-- Policies
CREATE SECURITY POLICY rls_Policies_Policy
    ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Policies
WITH (STATE = ON, SCHEMABINDING = ON);

-- Cases
CREATE SECURITY POLICY rls_Cases_Policy
    ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.NonComplianceCases
WITH (STATE = ON, SCHEMABINDING = ON);

-- ComplianceStates
CREATE SECURITY POLICY rls_ComplianceStates_Policy
    ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ComplianceStates
WITH (STATE = ON, SCHEMABINDING = ON);

-- AuditLogs
CREATE SECURITY POLICY rls_AuditLogs_Policy
    ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.AuditLogs
WITH (STATE = ON, SCHEMABINDING = ON);

-- EventEnvelopes
CREATE SECURITY POLICY rls_EventEnvelopes_Policy
    ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.EventEnvelopes
WITH (STATE = ON, SCHEMABINDING = ON);

-- OutboxMessages (if tenant-scoped)
-- CREATE SECURITY POLICY rls_OutboxMessages_Policy
--     ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.OutboxMessages
-- WITH (STATE = ON, SCHEMABINDING = ON);

GO

-- Test RLS setup
-- Before running queries, execute: EXEC sp_set_session_context @key=N'TenantId', @value=N'<your-tenant-id-here>';
```

### 2.2 Performance Indexes (Beyond Primary Keys)

```sql
USE InsureXDb;
GO

-- Assets
CREATE INDEX IX_Assets_TenantId_Status 
    ON dbo.Assets(TenantId, Status)
    INCLUDE (AssetIdentifier, FinancedAmount, CreatedUtc);

CREATE INDEX IX_Assets_TenantId_CreatedUtc 
    ON dbo.Assets(TenantId, CreatedUtc DESC);

CREATE INDEX IX_Assets_AssetIdentifier 
    ON dbo.Assets(TenantId, AssetIdentifier);

-- Policies
CREATE INDEX IX_Policies_TenantId_Status 
    ON dbo.Policies(TenantId, Status)
    INCLUDE (PolicyNumber, EffectiveDate, ExpiryDate);

CREATE INDEX IX_Policies_AssetId 
    ON dbo.Policies(AssetId, TenantId);

CREATE INDEX IX_Policies_PaymentStatus 
    ON dbo.Policies(TenantId, PaymentStatus)
    INCLUDE (PolicyNumber, LastPremiumPaymentDate);

-- ComplianceStates
CREATE INDEX IX_ComplianceStates_TenantId_Status 
    ON dbo.ComplianceStates(TenantId, Status)
    INCLUDE (AssetId, LastEvaluatedUtc);

CREATE INDEX IX_ComplianceStates_AssetId 
    ON dbo.ComplianceStates(AssetId);

-- NonComplianceCases
CREATE INDEX IX_NonComplianceCases_TenantId_Status 
    ON dbo.NonComplianceCases(TenantId, Status)
    INCLUDE (AssetId, CaseNumber, CreatedUtc);

CREATE INDEX IX_NonComplianceCases_AssetId 
    ON dbo.NonComplianceCases(AssetId);

CREATE INDEX IX_NonComplianceCases_AssignedToUserId 
    ON dbo.NonComplianceCases(TenantId, AssignedToUserId);

-- AuditLogs
CREATE INDEX IX_AuditLogs_TenantId_CreatedUtc 
    ON dbo.AuditLogs(TenantId, CreatedUtc DESC);

CREATE INDEX IX_AuditLogs_EntityType_EntityId 
    ON dbo.AuditLogs(EntityType, EntityId, TenantId);

CREATE INDEX IX_AuditLogs_ActorUserId 
    ON dbo.AuditLogs(ActorUserId, TenantId);

-- EventEnvelopes
CREATE INDEX IX_EventEnvelopes_TenantId_Status 
    ON dbo.EventEnvelopes(TenantId, Status)
    INCLUDE (EventType, CreatedUtc);

CREATE INDEX IX_EventEnvelopes_SourceEventId 
    ON dbo.EventEnvelopes(TenantId, SourceEventId);

-- OutboxMessages
CREATE INDEX IX_OutboxMessages_Status 
    ON dbo.OutboxMessages(Status)
    INCLUDE (EventType, CreatedUtc);

CREATE INDEX IX_OutboxMessages_Topic 
    ON dbo.OutboxMessages(Topic, Status);

GO
```

### 2.3 Stored Procedures (Optional – for Heavy Reporting)

If you want stored procedures for performance-critical reporting, define them here:

```sql
USE InsureXDb;
GO

-- Get compliance summary for a tenant (fast reporting)
CREATE PROCEDURE sp_GetComplianceSummary
    @TenantId UNIQUEIDENTIFIER,
    @FromDate DATETIME2,
    @ToDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    
    EXEC sp_set_session_context @key=N'TenantId', @value=@TenantId;
    
    SELECT
        cs.[Status],
        COUNT(*) AS AssetCount,
        COUNT(DISTINCT nc.Id) AS ActiveCaseCount
    FROM dbo.ComplianceStates cs
    LEFT JOIN dbo.NonComplianceCases nc ON cs.Id = nc.ComplianceStateId
        AND nc.[Status] IN (1, 2, 3)  -- Open, InProgress, Escalated
    WHERE cs.TenantId = @TenantId
        AND cs.LastEvaluatedUtc >= @FromDate
        AND cs.LastEvaluatedUtc <= @ToDate
    GROUP BY cs.[Status];
END

GO

-- Get outstanding cases (for dashboard)
CREATE PROCEDURE sp_GetOutstandingCases
    @TenantId UNIQUEIDENTIFIER,
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    
    EXEC sp_set_session_context @key=N'TenantId', @value=@TenantId;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    SELECT
        nc.Id,
        nc.CaseNumber,
        nc.AssetId,
        a.AssetIdentifier,
        nc.NonComplianceReason,
        nc.[Status],
        nc.Severity,
        nc.AssignedToUserId,
        nc.DueDate,
        nc.CreatedUtc,
        (SELECT COUNT(*) FROM dbo.NonComplianceCases 
         WHERE TenantId = @TenantId AND [Status] IN (1, 2, 3)) AS TotalRecords
    FROM dbo.NonComplianceCases nc
    INNER JOIN dbo.Assets a ON nc.AssetId = a.Id
    WHERE nc.TenantId = @TenantId
        AND nc.[Status] IN (1, 2, 3)  -- Open, InProgress, Escalated
    ORDER BY nc.DueDate ASC, nc.CreatedUtc DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END

GO

-- Audit trail for a specific asset
CREATE PROCEDURE sp_GetAssetAuditTrail
    @TenantId UNIQUEIDENTIFIER,
    @AssetId UNIQUEIDENTIFIER,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    
    EXEC sp_set_session_context @key=N'TenantId', @value=@TenantId;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    SELECT
        al.Id,
        al.[Action],
        al.EntityType,
        al.ActorUserId,
        al.ActorSystem,
        al.OldValues,
        al.NewValues,
        al.CorrelationId,
        al.CreatedUtc,
        (SELECT COUNT(*) FROM dbo.AuditLogs 
         WHERE TenantId = @TenantId AND EntityId = CAST(@AssetId AS NVARCHAR(MAX))) AS TotalRecords
    FROM dbo.AuditLogs al
    WHERE al.TenantId = @TenantId
        AND al.EntityId = CAST(@AssetId AS NVARCHAR(MAX))
    ORDER BY al.CreatedUtc DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END

GO
```

---

## PART 3: SEED DATA (TESTING)

Run these after migrations to set up test data:

```sql
USE InsureXDb;
GO

-- Insert test tenant (Bank)
DECLARE @TenantId UNIQUEIDENTIFIER = NEWID();
DECLARE @TenantCode NVARCHAR(50) = 'test-bank-1';
DECLARE @TenantName NVARCHAR(200) = 'Test Bank Limited';

INSERT INTO dbo.Tenants (Id, Code, Name, Type, Status, CreatedUtc, ModifiedUtc)
VALUES (@TenantId, @TenantCode, @TenantName, 1, 1, GETUTCDATE(), GETUTCDATE());

-- Insert Organisation for the bank
DECLARE @OrgId UNIQUEIDENTIFIER = NEWID();
INSERT INTO dbo.Organisations (Id, TenantId, Name, Type, Status, CreatedUtc, ModifiedUtc)
VALUES (@OrgId, @TenantId, @TenantName, 1, 1, GETUTCDATE(), GETUTCDATE());

-- Insert test user
DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
INSERT INTO dbo.Users (Id, TenantId, OrganisationId, UserName, Email, PasswordHash, Status, LastLoginUtc, CreatedUtc)
VALUES (
    @UserId,
    @TenantId,
    @OrgId,
    'testuser',
    'testuser@bank.local',
    'hashed_password_here',  -- This would be set via Identity API in production
    1,
    NULL,
    GETUTCDATE()
);

-- Insert test assets
DECLARE @AssetId1 UNIQUEIDENTIFIER = NEWID();
INSERT INTO dbo.Assets (Id, TenantId, BankOrganisationId, AssetType, AssetIdentifier, RegistrationNumber, 
                        FinancedAmount, BorrowerReference, LoanStartDate, LoanEndDate, Status, CreatedUtc, ModifiedUtc)
VALUES (
    @AssetId1,
    @TenantId,
    @OrgId,
    'Motor',
    'WBADT43452G297186',  -- Sample VIN
    'GP01 XYZ',
    250000,
    'CUST_001',
    '2024-01-15',
    '2027-01-15',
    1,  -- Active
    GETUTCDATE(),
    GETUTCDATE()
);

DECLARE @AssetId2 UNIQUEIDENTIFIER = NEWID();
INSERT INTO dbo.Assets (Id, TenantId, BankOrganisationId, AssetType, AssetIdentifier, RegistrationNumber,
                        FinancedAmount, BorrowerReference, LoanStartDate, LoanEndDate, Status, CreatedUtc, ModifiedUtc)
VALUES (
    @AssetId2,
    @TenantId,
    @OrgId,
    'Motor',
    'WBADT43452G297187',
    'GP01 ABC',
    320000,
    'CUST_002',
    '2024-02-01',
    '2027-02-01',
    1,
    GETUTCDATE(),
    GETUTCDATE()
);

-- Insert test insurer tenant
DECLARE @InsurerTenantId UNIQUEIDENTIFIER = NEWID();
INSERT INTO dbo.Tenants (Id, Code, Name, Type, Status, CreatedUtc, ModifiedUtc)
VALUES (@InsurerTenantId, 'insurer-aig', 'AIG Insurance', 2, 1, GETUTCDATE(), GETUTCDATE());

DECLARE @InsurerOrgId UNIQUEIDENTIFIER = NEWID();
INSERT INTO dbo.Organisations (Id, TenantId, Name, Type, Status, CreatedUtc, ModifiedUtc)
VALUES (@InsurerOrgId, @InsurerTenantId, 'AIG Insurance', 2, 1, GETUTCDATE(), GETUTCDATE());

-- Insert policies for the assets
INSERT INTO dbo.Policies (Id, TenantId, AssetId, InsurerOrganisationId, PolicyNumber, ProductType, 
                         Status, EffectiveDate, ExpiryDate, InsuredValue, PremiumAmount, PaymentStatus, 
                         LastPremiumPaymentDate, CreatedUtc, ModifiedUtc)
VALUES
    (NEWID(), @TenantId, @AssetId1, @InsurerOrgId, 'AIG-2024-001', 'Comprehensive', 
     1, '2024-01-15', '2025-01-14', 250000, 15000, 1, '2024-12-01', GETUTCDATE(), GETUTCDATE()),
    (NEWID(), @TenantId, @AssetId2, @InsurerOrgId, 'AIG-2024-002', 'Comprehensive', 
     1, '2024-02-01', '2025-02-01', 320000, 18000, 1, '2024-12-15', GETUTCDATE(), GETUTCDATE());

-- Insert compliance states
INSERT INTO dbo.ComplianceStates (Id, TenantId, AssetId, Status, NonComplianceReason, 
                                 LastEvaluatedUtc, LastChangedUtc, CreatedUtc, ModifiedUtc)
VALUES
    (NEWID(), @TenantId, @AssetId1, 1, NULL, GETUTCDATE(), GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),  -- Compliant
    (NEWID(), @TenantId, @AssetId2, 1, NULL, GETUTCDATE(), GETUTCDATE(), GETUTCDATE(), GETUTCDATE());   -- Compliant

GO

-- Verify seed data
SELECT COUNT(*) AS TenantCount FROM dbo.Tenants;
SELECT COUNT(*) AS AssetCount FROM dbo.Assets;
SELECT COUNT(*) AS PolicyCount FROM dbo.Policies;
SELECT COUNT(*) AS ComplianceStateCount FROM dbo.ComplianceStates;

GO
```

---

## PART 4: MAINTENANCE & MONITORING

### 4.1 Database Growth Check

```sql
-- Check database size
USE InsureXDb;
GO

EXEC sp_spaceused;

GO

-- Check table sizes
SELECT
    s.Name AS SchemaName,
    t.Name AS TableName,
    SUM(p.rows) AS RowCount,
    SUM(au.total_pages) * 8 / 1024.0 AS TotalSizeMB
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
INNER JOIN sys.partitions p ON t.object_id = p.object_id
INNER JOIN sys.allocation_units au ON p.partition_id = au.container_id
GROUP BY s.Name, t.Name
ORDER BY SUM(au.total_pages) DESC;

GO
```

### 4.2 Index Fragmentation

```sql
-- Check index fragmentation
USE InsureXDb;
GO

SELECT
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.avg_fragmentation_in_percent AS FragmentationPercent,
    ips.page_count AS PageCount
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id
    AND ips.index_id = i.index_id
WHERE ips.page_count > 1000
    AND ips.avg_fragmentation_in_percent > 10
ORDER BY ips.avg_fragmentation_in_percent DESC;

GO

-- Rebuild fragmented indexes (if fragmentation > 30%)
-- Reorganize if 10-30%
ALTER INDEX index_name ON table_name REBUILD;
```

### 4.3 Query Performance Monitoring

```sql
-- Top 10 slowest queries
USE InsureXDb;
GO

SELECT TOP 10
    qs.execution_count,
    qs.total_elapsed_time / 1000000.0 AS TotalElapsedTime_Sec,
    qs.total_elapsed_time / qs.execution_count / 1000.0 AS AvgElapsedTime_MS,
    SUBSTRING(st.text, 1, 100) AS QueryText
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
ORDER BY qs.total_elapsed_time DESC;

GO
```

### 4.4 Backup Strategy (LocalDB)

For LocalDB, backups are automatic in System center/Visual Studio. For production on SQL Server 2022:

```sql
-- Full backup
BACKUP DATABASE InsureXDb
TO DISK = 'C:\Backups\InsureXDb_FULL_20260103.bak'
WITH COMPRESSION;

-- Transaction log backup (daily, after full backup)
BACKUP LOG InsureXDb
TO DISK = 'C:\Backups\InsureXDb_LOG_20260103.trn'
WITH COMPRESSION;

-- Restore from backup
RESTORE DATABASE InsureXDb
FROM DISK = 'C:\Backups\InsureXDb_FULL_20260103.bak'
WITH REPLACE;
```

---

## PART 5: DEVELOPMENT WORKFLOW

### 5.1 Running Migrations Locally

```bash
# From your ASP.NET Core project directory
cd src/InsureX.Api

# Add a new migration (if schema changes)
dotnet ef migrations add AddNewFeature -p ../InsureX.Infrastructure

# Apply migrations to local database
dotnet ef database update

# Generate migration script (for code review)
dotnet ef migrations script > ../migrations.sql
```

### 5.2 Testing RLS Locally

```sql
-- In SSMS, open a new query window:

USE InsureXDb;
GO

-- Set session context for Tenant A
DECLARE @TenantA UNIQUEIDENTIFIER = (SELECT Id FROM dbo.Tenants WHERE Code = 'test-bank-1');
EXEC sp_set_session_context @key=N'TenantId', @value=@TenantA;

-- This query should only return assets from Tenant A
SELECT Id, AssetIdentifier FROM dbo.Assets;

-- Now switch to a different tenant
DECLARE @TenantB UNIQUEIDENTIFIER = (SELECT Id FROM dbo.Tenants WHERE Code = 'insurer-aig');
EXEC sp_set_session_context @key=N'TenantId', @value=@TenantB;

-- This query should only return assets from Tenant B (should be empty if no assets for this tenant)
SELECT Id, AssetIdentifier FROM dbo.Assets;

GO
```

---

## PART 6: PRODUCTION CHECKLIST

- [ ] Use SQL Server 2022 Edition (Standard or Enterprise)
- [ ] Enable TLS 1.2+ for connections
- [ ] Configure SQL Server authentication securely (disable sa account)
- [ ] Implement automated backups (daily full + transaction log backups)
- [ ] Monitor database growth + plan for scaling
- [ ] Enable Query Store for performance insights
- [ ] Set up alerts for disk space + CPU
- [ ] Document RLS policies + security model
- [ ] Test disaster recovery procedures quarterly
- [ ] Implement row-level security (done in this guide)
- [ ] Configure audit logging for compliance
- [ ] Use connection pooling in application code
- [ ] Encrypt database backups
- [ ] Review and optimize slow queries regularly

---

## USEFUL SCRIPTS

### List all tables with row counts
```sql
USE InsureXDb;
GO

SELECT
    s.Name,
    t.Name,
    SUM(p.rows) AS RowCount
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id = 0
GROUP BY s.Name, t.Name
ORDER BY RowCount DESC;

GO
```

### Rebuild all indexes
```sql
USE InsureXDb;
GO

EXEC sp_MSForEachTable 'DBCC DBREINDEX (''?'')';

GO
```

### Check for missing indexes
```sql
USE InsureXDb;
GO

SELECT
    migs.user_seeks + migs.user_scans + migs.user_lookups AS TotalReads,
    mid.equality_columns,
    mid.inequality_columns,
    mid.included_columns
FROM sys.dm_db_missing_index_details mid
INNER JOIN sys.dm_db_missing_index_groups_stats migs 
    ON mid.index_handle = migs.index_handle
WHERE database_id = DB_ID()
ORDER BY TotalReads DESC;

GO
```

---

## REFERENCE

- **SQL Server 2022 Docs:** https://learn.microsoft.com/en-us/sql/sql-server/what-s-new-in-sql-server-2022
- **Row-Level Security:** https://learn.microsoft.com/en-us/sql/relational-databases/security/row-level-security
- **EF Core Migrations:** https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/
- **SQL Server LocalDB:** https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb

---

**Document Version:** 1.0  
**Last Updated:** January 2026
