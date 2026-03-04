# Phase 2, Part 2: TenantId Propagation to Domain Entities

## Objective
After establishing the `Tenant` and `Organization` data models (Part 1), the next step was to propagate the `TenantId` foreign key reference across all core domain entity models in the `IAPR_Data` layer.

## Strategy Implemented

Rather than modifying the database schema directly (since these are plain C# DTO/model objects used by ADO.NET Data Providers), we added a `public int? TenantId { get; set; }` property to each relevant domain entity class. This makes `TenantId` a first-class field that:
- Can be populated when reading data from the database (once the column is added to the physical tables).
- Can be used by future middleware and query filters to enforce tenant isolation.

## Entities Updated

The following 10 entity classes were updated:

| Entity File | Class |
|---|---|
| `Classes/Policy/Policy.cs` | `Policy` |
| `Classes/AssetTypes/Vehicle_Asset.cs` | `Vehicle_Asset` |
| `Classes/AssetTypes/Property_Asset.cs` | `Property_Asset` |
| `Classes/AssetTypes/ElectronicEquipment_Asset.cs` | `ElectronicEquipment_Asset` |
| `Classes/AssetTypes/Aviation_Asset.cs` | `Aviation_Asset` |
| `Classes/AssetTypes/Machinery_Asset.cs` | `Machinery_Asset` |
| `Classes/AssetTypes/Stock_Asset.cs` | `Stock_Asset` |
| `Classes/AssetTypes/KeymanInsurance_Asset.cs` | `KeymanInsurance_Asset` |
| `Classes/AssetTypes/Watercraft_Asset.cs` | `Watercraft_Asset` |
| `Classes/AssetTypes/PlantEquipment_Asset.cs` | `PlantEquipment_Asset` |

## Result
The `IAPR_Data` project continues to build with 0 errors. The domain entity layer is now structurally prepared for the upcoming `TenantContext` middleware and EF Global Query Filter enforcement.
