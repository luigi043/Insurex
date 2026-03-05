# Start InsureX Modern API and Frontend

# 1. Start API (Port 5000 by default)
Write-Host "Starting InsureX.Api..." -ForegroundColor Cyan
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project InsureX.Api/InsureX.Api.csproj"

# 2. Start Frontend (Vite)
Write-Host "Starting InsureX.Frontend..." -ForegroundColor Cyan
Set-Location -Path "InsureX.Frontend"
npm run dev
