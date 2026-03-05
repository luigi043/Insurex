# InsureX Setup and Run Guide

This guide provides instructions on how to build and run the InsureX application, specifically focusing on the `IAPR_Web` frontend and the legacy APIs.

## Prerequisites

- `.NET Framework 4.8` Developer Pack
- `NuGet` (for package restoration)
- `MSBuild` (included with Visual Studio 2022)
- `IIS Express`
- `LocalDB` installed with checking that the (localdb)\MSSQLLocalDB instance exists.

## 1. Restoring Packages

If you are running the project for the first time or after a clean clone, you need to restore the NuGet packages.

Open PowerShell or Developer Command Prompt in the `InsureX` folder (`C:\Users\cluiz\projectx\Insurex`):

```powershell
# Restore solution packages
.\nuget.exe restore Insured_Assest_Protection_Register.sln

# Restore web frontend packages
.\nuget.exe restore IAPR_Web\packages.config -PackagesDirectory packages
```

## 2. Building the Solution

Due to the legacy `.NET Framework 4.8` WebForms/WCF architecture, the project must be compiled using the full `MSBuild` rather than the `dotnet` CLI.

From the developer prompt:

```powershell
MSBuild.exe Insured_Assest_Protection_Register.sln /p:Configuration=Debug
```

## 3. Starting the Project (Web Frontend + API)

A script has been provided to automatically launch **both** the frontend and the WCF API at the same time using IIS Express.

To execute the script, open PowerShell in the project root folder (`C:\Users\cluiz\projectx\Insurex`) and run:

```powershell
.\start_all.ps1
```

- The **Web Frontend** will be hosted on `http://localhost:12857/` (which defaults to `account/login.aspx`).
- The **WCF API Services** will be concurrently hosted on `http://localhost:12858/`

To completely cleanly stop the servers if they get stuck holding a loose process, you can run `Stop-Process -Name iisexpress -Force` in PowerShell.

## 4. Default Test User / Admin Seed

Since the modernization project wiped the legacy user data in favor of ASP.NET Core Identity, the local database may be unseeded during evaluation.

To easily set up a default Administrator account, send a test POST request to the API's setup endpoint once the server is running:

```powershell
Invoke-RestMethod -Method Post -Uri "http://localhost:5000/api/admin/setup"
```

Once executed successfully, you can log into the React frontend using the following default credentials:

- **Email/Username**: `admin@insurex.local`
- **Password**: `Admin123!`
