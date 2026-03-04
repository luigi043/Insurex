# Fixing Backend and API Build Errors

## Overview
Based on your request to "fix the back-end and api", I investigated the build errors in the `InsureX` solution, specifically focusing on the data layer (`IAPR_Data`) and the REST API (`IAPR_API`).

## Changes Made
1. **Restored NuGet Packages**:
   - The [.sln](file:///c:/Users/cluiz/projectx/Insurex/Insured_Assest_Protection_Register.sln) file was missing its [packages.config](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Web/packages.config) restorations, so we downloaded [nuget.exe](file:///c:/Users/cluiz/projectx/Insurex/nuget.exe) and restored the `packages` folder.
   
2. **Fixed `IAPR_Data` References**:
   - `Newtonsoft.Json`: Updated the [.csproj](file:///c:/Users/cluiz/projectx/Insurex/IAPR_API/IAPRAPI.csproj) `HintPath` from missing `bin\Debug\Newtonsoft.Json.dll` to correctly reference `..\packages\Newtonsoft.Json.13.0.1\lib\net45\Newtonsoft.Json.dll`.
   - `Microsoft.ApplicationBlocks.Data`: The reference was hardcoded to a missing manual copy in `bin\Debug\`. We installed the package via NuGet into the `packages` directory and updated the `HintPath` in the [.csproj](file:///c:/Users/cluiz/projectx/Insurex/IAPR_API/IAPRAPI.csproj) file to point to it correctly.

## Verification
- We ran MSBuild against the `IAPR_API.csproj` layout and confirmed that `IAPR_API` and `IAPR_Data` both compile **successfully** with 0 errors.

## Frontend Updates (`IAPR_Web`)
1. **Restored frontend NuGet Packages**:
   - Recovered missing packages defined in [packages.config](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Web/packages.config) for the `IAPR_Web` directory.
2. **Missing third-party UI controls**:
   - `AjaxControlToolkit` vs 20.1 was missing from the repository entirely. Downloaded it from NuGet and updated the project `HintPath` to use the NuGet package.
   - `FusionCharts` had dead references to a `bin` directory that was ignored by source control. Restored `FusionCharts.Visualization` and wrappers via NuGet and updated 10 `HintPath` references.
3. **Running the Application**:
   - `IAPR_Web` now complies 100% successfully on MSBuild.
   - Hosted the `IAPR_Web` instance using IIS Express on `http://localhost:12857/`. The root page loads successfully and redirects to `account/login.aspx`.

## Phase 0: Emergency Security Remediation

To address the immediate high-risk security findings, I have completed the following steps:

1. **Removed Source Control Pollutants:** Created a [.gitignore](file:///c:/Users/cluiz/projectx/Insurex/.gitignore) to prevent `bin/`, `obj/`, and `.vs/` from being committed to Git.
2. **Remediated Hardcoded AES Keys:** Updated [IAPR_Data\Utils\CryptorEngine.cs](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Data/Utils/CryptorEngine.cs) to remove the hardcoded symmetric keys. The application now expects `AesCryptoKey` to be provided securely via ConfigurationManager.
3. **Removed Hardcoded Passwords:** Stripped the plaintext `SMTPServerPassword` and `PaymentGatewayPassword`/`userId` from [IAPR_Web\Web.config](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Web/Web.config) and [IAPR_API\Web.config](file:///c:/Users/cluiz/projectx/Insurex/IAPR_API/Web.config). These values are now defined as placeholders.
4. **Enforced Security Policies:** Updated the [Web.config](file:///c:/Users/cluiz/projectx/Insurex/IAPR_API/Web.config) files to enforce `httpOnlyCookies` and set `requireSSL="true"` for forms authentication.
5. **Git Commit:** Committed these changes to the source repository.

## Phase 1: Authentication & Data Protection

Because the legacy `IAPR` database `.sql` schemas were not provided and the previous developer's migration attempt in `src/InsureX.Web/` was deleted in git commit `c4e4b9d`, we proceeded with **Option A**: A Code-First EF implementation mapped directly into the legacy services.

1. **Integrated ASP.NET Identity:** Injected `EntityFramework` and `Microsoft.AspNet.Identity.EntityFramework` NuGet packages into the legacy `IAPR_Data` and `IAPR_Web` layers.
2. **Scaffolded IdentityModels:** Hand-crafted [IdentityModels.cs](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Data/Classes/IdentityModels.cs) in `IAPR_Data` to emulate the older `CurrentUser` signature (with fields like `vcName`, `iUser_Type_Id`, `LegacyUserId`) using the secure `IdentityUser` foundation.
3. **Replaced Legacy Passwords:** Completely stubbed out the missing `spGet_AuthenticateUser` and `spUpd_Change_Password_Status` stored procedures inside [User_Provider.cs](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Data/Providers/User_Provider.cs). Authentication queries now securely run via `UserManager<ApplicationUser>.Find()`.
4. **Enabled Secure Cookies:** Hooked [Login.aspx.cs](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Web/Account/Login.aspx.cs) to issue a secure OWIN `ApplicationCookie` that holds standard `ClaimsIdentity` claims instead of just pushing user objects to a predictable [Session](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Data/Providers/User_Provider.cs#65-84) variable.
5. **Seeded Test Admin:** Added a Code-First Database Seeder to [Global.asax.cs](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Web/Global.asax.cs) > [Application_Start](file:///c:/Users/cluiz/projectx/Insurex/IAPR_Web/Global.asax.cs#17-44) ensuring `admin@insurex.com` (`Password123!`) is automatically created exactly if missing on local machines.

The entire solution passed `MSBuild` with 0 Errors using these modernized layers.
