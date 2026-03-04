# Phase 2, Part 1: Tenant and Organization Model Implementation

## Objective
The first step of implementing our Multi-Tenant architecture was to establish the physical domain models that will isolate users and data across multiple organizations.

## Strategy Implemented
1. **MultiTenant Models:** Created `IAPR_Data/Classes/MultiTenantModels.cs` outlining two core classes:
   - `Tenant`: Represents the top-level corporate entity (e.g., a specific Bank or Underwriter).
   - `Organization`: Represents hierarchical subsidiaries or branches securely belonging to a `Tenant`.
2. **EF Configuration:** Registered `Tenants` and `Organizations` as primary `DbSet` tables inside `ApplicationDbContext`.
3. **Identity Binding:** We extended the `ApplicationUser` model (inside `IdentityModels.cs`) to incorporate `TenantId` and `OrganizationId` Foreign Keys. This structurally guarantees that whenever users are authenticated and logged in, their specific Tenant and Organization IDs are always rigidly determinable.
4. **Database Initialization & Seeding:** Modified `IAPR_Web/Global.asax.cs` to explicitly execute `Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>())`. 
   - Before the system creates the default admin user account (`admin@insurex.com`), it sequentially ensures a default "System Default Tenant" and "Headquarters" Organization exist in the database.
   - The admin user is then anchored to this Tenant/Organization to prevent foreign-key null violations.

## Result
The Data Layer compilation succeeded. EF Code-First is completely configured to manage the newly bounded Tenant/Organization entity relations.
