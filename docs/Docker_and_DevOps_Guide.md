# InsureX – Docker & DevOps Deployment Guide

---

## PART 1: LOCAL DEVELOPMENT WITH DOCKER

### 1.1 Prerequisites

- Docker Desktop installed
- Visual Studio 2022 or VS Code
- Docker Compose installed (included with Docker Desktop)

### 1.2 Project Structure

```
InsureX/
├── docker/
│   ├── Dockerfile.api       (ASP.NET Core API)
│   ├── Dockerfile.web       (React frontend)
│   ├── Dockerfile.db        (SQL Server)
│   └── .dockerignore
├── docker-compose.yml       (Orchestrate all services)
├── docker-compose.override.yml (Local dev overrides)
└── .dockerignore
```

### 1.3 Dockerfile for API

**docker/Dockerfile.api**
```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and project files
COPY ["InsureX.sln", "."]
COPY ["src/InsureX.Api/InsureX.Api.csproj", "src/InsureX.Api/"]
COPY ["src/InsureX.Application/InsureX.Application.csproj", "src/InsureX.Application/"]
COPY ["src/InsureX.Domain/InsureX.Domain.csproj", "src/InsureX.Domain/"]
COPY ["src/InsureX.Infrastructure/InsureX.Infrastructure.csproj", "src/InsureX.Infrastructure/"]
COPY ["src/InsureX.Shared/InsureX.Shared.csproj", "src/InsureX.Shared/"]

# Restore
RUN dotnet restore

# Copy source code
COPY ["src/", "src/"]

# Build
WORKDIR "/src/src/InsureX.Api"
RUN dotnet build "InsureX.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "InsureX.Api.csproj" -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Set environment
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD dotnet --version || exit 1

EXPOSE 80

ENTRYPOINT ["dotnet", "InsureX.Api.dll"]
```

### 1.4 Dockerfile for Frontend

**docker/Dockerfile.web**
```dockerfile
# Stage 1: Build React
FROM node:18-alpine AS build

WORKDIR /app

# Copy package files
COPY ["InsureX.Frontend/package*.json", "./"]

# Install dependencies
RUN npm ci

# Copy source
COPY ["InsureX.Frontend/", "./"]

# Build
RUN npm run build

# Stage 2: Serve with nginx
FROM nginx:alpine

# Copy nginx config
COPY docker/nginx.conf /etc/nginx/nginx.conf

# Copy built app
COPY --from=build /app/dist /usr/share/nginx/html

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD wget --quiet --tries=1 --spider http://localhost/health || exit 1

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

**docker/nginx.conf**
```nginx
user nginx;
worker_processes auto;
error_log /var/log/nginx/error.log warn;
pid /var/run/nginx.pid;

events {
    worker_connections 1024;
}

http {
    include /etc/nginx/mime.types;
    default_type application/octet-stream;

    log_format main '$remote_addr - $remote_user [$time_local] "$request" '
                    '$status $body_bytes_sent "$http_referer" '
                    '"$http_user_agent" "$http_x_forwarded_for"';

    access_log /var/log/nginx/access.log main;

    sendfile on;
    tcp_nopush on;
    tcp_nodelay on;
    keepalive_timeout 65;
    types_hash_max_size 2048;
    client_max_body_size 100M;

    gzip on;
    gzip_vary on;
    gzip_types text/plain text/css text/xml text/javascript 
               application/x-javascript application/javascript application/xml+rss;

    server {
        listen 80;
        server_name _;
        root /usr/share/nginx/html;
        index index.html;

        # SPA routing: route all requests to index.html
        location / {
            try_files $uri $uri/ /index.html;
        }

        # API proxy (if using separate frontend + API)
        location /api/ {
            proxy_pass http://api:80;
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection 'upgrade';
            proxy_set_header Host $host;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }

        # Health check endpoint
        location /health {
            access_log off;
            return 200 'OK';
        }

        # Cache static assets
        location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|eot)$ {
            expires 7d;
            add_header Cache-Control "public, immutable";
        }
    }
}
```

### 1.5 docker-compose.yml

```yaml
version: '3.8'

services:
  # SQL Server Database
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: insurex-sqlserver
    environment:
      SA_PASSWORD: YourStrongPassword123!
      ACCEPT_EULA: "Y"
      MSSQL_PID: "Developer"
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    networks:
      - insurex_network
    healthcheck:
      test: /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrongPassword123! -Q "SELECT 1"
      interval: 10s
      timeout: 5s
      retries: 5

  # ASP.NET Core API
  api:
    build:
      context: .
      dockerfile: docker/Dockerfile.api
    container_name: insurex-api
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ASPNETCORE_URLS: http://+:80
      ConnectionStrings__DefaultConnection: "Server=sqlserver;Database=InsureXDb;User Id=sa;Password=YourStrongPassword123!;Encrypt=False;TrustServerCertificate=True;"
      Jwt__SecretKey: "your-super-secret-jwt-key-at-least-32-characters-long"
      Jwt__Issuer: "https://insurex.local"
      Jwt__Audience: "insurex-api"
    ports:
      - "5000:80"
    depends_on:
      sqlserver:
        condition: service_healthy
    volumes:
      - ./src:/src
    networks:
      - insurex_network
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s

  # React Frontend
  web:
    build:
      context: .
      dockerfile: docker/Dockerfile.web
    container_name: insurex-web
    environment:
      REACT_APP_API_URL: http://localhost:5000/api/v1
    ports:
      - "80:80"
    depends_on:
      - api
    networks:
      - insurex_network
    healthcheck:
      test: ["CMD", "wget", "--quiet", "--tries=1", "--spider", "http://localhost/health"]
      interval: 30s
      timeout: 10s
      retries: 3

networks:
  insurex_network:
    driver: bridge

volumes:
  sqlserver_data:
```

### 1.6 docker-compose.override.yml (Local Development)

```yaml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: docker/Dockerfile.api
    environment:
      ASPNETCORE_ENVIRONMENT: Development
    volumes:
      - ./src:/src
    ports:
      - "5000:80"
      - "5001:443"

  web:
    build:
      context: .
      dockerfile: docker/Dockerfile.web
    environment:
      REACT_APP_API_URL: http://localhost:5000/api/v1
      NODE_ENV: development
    ports:
      - "3000:80"
    volumes:
      - ./InsureX.Frontend/src:/app/src
      - /app/node_modules

  sqlserver:
    ports:
      - "1433:1433"
```

### 1.7 .dockerignore

```
**/.classpath
**/.dockerignore
**/.env
**/.git
**/.gitignore
**/.project
**/.settings
**/.toolstartsettings
**/.vs
**/.vscode
**/bin
**/build
**/dist
**/node_modules
**/npm-debug.log
**/obj
**/secrets.dev.yaml
**/values.dev.yaml
LICENSE
README.md
docker-compose*.yml
**/.git
**/.gitignore
**/.vs
**/.vscode
**/bin
**/obj
```

### 1.8 Running Locally

```bash
# Navigate to project root
cd InsureX

# Build images
docker-compose build

# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api
docker-compose logs -f web
docker-compose logs -f sqlserver

# Stop all services
docker-compose down

# Remove volumes (reset database)
docker-compose down -v
```

**Access:**
- Frontend: http://localhost
- API: http://localhost:5000
- SQL Server: localhost:1433 (sa / YourStrongPassword123!)

---

## PART 2: GITHUB ACTIONS CI/CD

### 2.1 Workflow for Testing & Building

**.github/workflows/build.yml**
```yaml
name: Build & Test

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main, develop]

jobs:
  test:
    runs-on: ubuntu-latest

    services:
      sqlserver:
        image: mcr.microsoft.com/mssql/server:2022-latest
        env:
          SA_PASSWORD: Password123!
          ACCEPT_EULA: Y
        options: >-
          --health-cmd "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Password123! -Q 'SELECT 1'"
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
        ports:
          - 1433:1433

    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'

      - name: Restore
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release --no-restore

      - name: Test
        run: dotnet test --configuration Release --no-build --verbosity normal
        env:
          ConnectionStrings__DefaultConnection: "Server=localhost,1433;Database=InsureXDb;User Id=sa;Password=Password123!;Encrypt=False;"

      - name: Publish
        run: dotnet publish src/InsureX.Api/InsureX.Api.csproj -c Release -o ./publish

      - name: Upload Artifacts
        uses: actions/upload-artifact@v3
        with:
          name: api-build
          path: ./publish
```

### 2.2 Workflow for Docker Push to ACR

**.github/workflows/deploy.yml**
```yaml
name: Deploy to Azure

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v3

      - name: Login to Azure
        uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}

      - name: Build and Push to ACR
        uses: azure/CLI@v1
        with:
          inlineScript: |
            az acr build \
              --registry ${{ secrets.REGISTRY_NAME }} \
              --image insurex-api:${{ github.sha }} \
              --image insurex-api:latest \
              --file docker/Dockerfile.api .

            az acr build \
              --registry ${{ secrets.REGISTRY_NAME }} \
              --image insurex-web:${{ github.sha }} \
              --image insurex-web:latest \
              --file docker/Dockerfile.web .

      - name: Deploy to App Service
        uses: azure/webapps-deploy@v2
        with:
          app-name: ${{ secrets.APPSERVICE_NAME }}
          images: |
            ${{ secrets.REGISTRY_LOGIN_SERVER }}/insurex-api:latest
            ${{ secrets.REGISTRY_LOGIN_SERVER }}/insurex-web:latest
```

**Set GitHub Secrets:**
```bash
# In GitHub repo settings: Settings > Secrets and variables > Actions

AZURE_CREDENTIALS          # Output from: az ad sp create-for-rbac
REGISTRY_NAME              # e.g., myregistry
REGISTRY_LOGIN_SERVER      # e.g., myregistry.azurecr.io
APPSERVICE_NAME            # e.g., insurex-prod
```

---

## PART 3: AZURE DEPLOYMENT

### 3.1 Create Azure Resources

```bash
# Variables
RESOURCE_GROUP="insurex-rg"
LOCATION="westeurope"
REGISTRY_NAME="insurexregistry"
APPSERVICE_PLAN="insurex-plan"
APPSERVICE_API="insurex-api"
APPSERVICE_WEB="insurex-web"
SQL_SERVER="insurex-sql-server"
SQL_DB="InsureXDb"

# Create resource group
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create Container Registry
az acr create --resource-group $RESOURCE_GROUP \
  --name $REGISTRY_NAME --sku Basic

# Create App Service Plan
az appservice plan create --name $APPSERVICE_PLAN \
  --resource-group $RESOURCE_GROUP --is-linux --sku B2

# Create App Service for API
az webapp create --resource-group $RESOURCE_GROUP \
  --plan $APPSERVICE_PLAN \
  --name $APPSERVICE_API \
  --deployment-container-image-name $REGISTRY_NAME.azurecr.io/insurex-api:latest

# Create App Service for Web
az webapp create --resource-group $RESOURCE_GROUP \
  --plan $APPSERVICE_PLAN \
  --name $APPSERVICE_WEB \
  --deployment-container-image-name $REGISTRY_NAME.azurecr.io/insurex-web:latest

# Create SQL Server
az sql server create --name $SQL_SERVER \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION \
  --admin-user sqladmin \
  --admin-password YourPassword123!

# Create SQL Database
az sql db create --server $SQL_SERVER \
  --resource-group $RESOURCE_GROUP \
  --name $SQL_DB \
  --edition Basic

# Configure firewall
az sql server firewall-rule create --server $SQL_SERVER \
  --resource-group $RESOURCE_GROUP \
  --name "AllowAzureServices" \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

### 3.2 Configure App Service Environment Variables

```bash
# For API App Service
az webapp config appsettings set --resource-group $RESOURCE_GROUP \
  --name $APPSERVICE_API \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    ConnectionStrings__DefaultConnection="Server=tcp:$SQL_SERVER.database.windows.net,1433;Initial Catalog=$SQL_DB;Persist Security Info=False;User ID=sqladmin;Password=YourPassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" \
    Jwt__SecretKey="your-production-jwt-secret-key" \
    Jwt__Issuer="https://$APPSERVICE_API.azurewebsites.net" \
    Jwt__Audience="insurex-api"

# For Web App Service
az webapp config appsettings set --resource-group $RESOURCE_GROUP \
  --name $APPSERVICE_WEB \
  --settings \
    REACT_APP_API_URL="https://$APPSERVICE_API.azurewebsites.net/api/v1"
```

### 3.3 Deploy from Container Registry

```bash
# Grant App Service access to ACR
az webapp identity assign --resource-group $RESOURCE_GROUP --name $APPSERVICE_API
PRINCIPAL_ID=$(az webapp identity show --resource-group $RESOURCE_GROUP \
  --name $APPSERVICE_API --query principalId --output tsv)

az role assignment create --assignee $PRINCIPAL_ID \
  --role AcrPull \
  --scope /subscriptions/{subscriptionId}/resourceGroups/$RESOURCE_GROUP/providers/Microsoft.ContainerRegistry/registries/$REGISTRY_NAME

# Configure container settings
az webapp config container set --name $APPSERVICE_API \
  --resource-group $RESOURCE_GROUP \
  --docker-custom-image-name "$REGISTRY_NAME.azurecr.io/insurex-api:latest" \
  --docker-registry-server-url "https://$REGISTRY_NAME.azurecr.io"
```

### 3.4 Azure Key Vault Integration

```bash
# Create Key Vault
az keyvault create --resource-group $RESOURCE_GROUP \
  --name insurex-keyvault \
  --location $LOCATION

# Add secrets
az keyvault secret set --vault-name insurex-keyvault \
  --name "ConnectionString" \
  --value "Server=tcp:insurex-sql-server.database.windows.net,1433;Initial Catalog=InsureXDb;User ID=sqladmin;Password=YourPassword123!;"

az keyvault secret set --vault-name insurex-keyvault \
  --name "JwtSecret" \
  --value "your-production-jwt-secret"

# Grant App Service access to Key Vault
az keyvault set-policy --name insurex-keyvault \
  --object-id $PRINCIPAL_ID \
  --secret-permissions get list
```

---

## PART 4: DATABASE MIGRATIONS IN PRODUCTION

### 4.1 Azure SQL Connection from Migrations

**Program.cs (using Key Vault)**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Use Key Vault in production
if (!builder.Environment.IsDevelopment())
{
    var keyVaultUrl = new Uri($"https://insurex-keyvault.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(
        keyVaultUrl,
        new DefaultAzureCredential());
}

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
```

### 4.2 Manual Migration (If Auto-Migration Fails)

```bash
# Generate migration script
dotnet ef migrations script -o migration-$(date +%Y%m%d).sql

# Apply in SSMS or via az sql script command
az sql db execute-script --server $SQL_SERVER \
  --database $SQL_DB \
  --admin-username sqladmin \
  --admin-password YourPassword123! \
  --input-file migration-20260103.sql
```

---

## PART 5: MONITORING & LOGGING

### 5.1 Application Insights Setup

**Program.cs**
```csharp
builder.Services.AddApplicationInsightsTelemetry(
    builder.Configuration["ApplicationInsights:InstrumentationKey"]);

builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.ApplicationInsights(
            new TelemetryClient(),
            TelemetryConverter.Traces)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));
```

**appsettings.Production.json**
```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-key-here"
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "ApplicationInsights",
        "Args": {
          "textFormatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
        }
      }
    ]
  }
}
```

### 5.2 Health Checks

```csharp
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "Database")
    .AddAzureServiceBus(serviceBusConnection, name: "ServiceBus");

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/detailed", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
```

---

## PART 6: SCALING & HIGH AVAILABILITY

### 6.1 Auto-Scale App Service

```bash
az monitor autoscale create --resource-group $RESOURCE_GROUP \
  --resource-name $APPSERVICE_API \
  --resource-type "Microsoft.Web/serverfarms" \
  --min-count 2 --max-count 10 \
  --count-based-autoscale-rule "Percentage CPU > 70 add 1" \
  --count-based-autoscale-rule "Percentage CPU < 25 remove 1"
```

### 6.2 Traffic Manager (Multi-Region)

```bash
az network traffic-manager profile create \
  --name insurex-traffic-mgr \
  --resource-group $RESOURCE_GROUP \
  --routing-method Geographic

az network traffic-manager endpoint create \
  --name api-westeurope \
  --profile-name insurex-traffic-mgr \
  --resource-group $RESOURCE_GROUP \
  --type azureEndpoints \
  --target $APPSERVICE_API.azurewebsites.net \
  --geo-mapping "geo-eu"
```

---

## PART 7: PRODUCTION CHECKLIST

- [ ] Docker images build without errors
- [ ] All environment variables configured
- [ ] Secrets in Key Vault (not in config)
- [ ] HTTPS enabled on App Services
- [ ] Database firewall rules configured
- [ ] Backup strategy in place
- [ ] Application Insights enabled
- [ ] Health checks working
- [ ] Load testing passed
- [ ] Security scan completed
- [ ] Rollback plan documented
- [ ] On-call runbook prepared

---

## USEFUL COMMANDS

### Docker
```bash
# Build image
docker build -f docker/Dockerfile.api -t insurex-api:latest .

# Push to registry
docker tag insurex-api:latest myregistry.azurecr.io/insurex-api:latest
docker push myregistry.azurecr.io/insurex-api:latest

# Run container
docker run -p 5000:80 \
  -e ConnectionStrings__DefaultConnection="..." \
  insurex-api:latest

# View logs
docker logs <container-id>

# Inspect
docker inspect <container-id>
```

### Azure CLI
```bash
# List all resources
az resource list --resource-group $RESOURCE_GROUP

# View App Service config
az webapp config show --name $APPSERVICE_API --resource-group $RESOURCE_GROUP

# SSH into container
az webapp ssh --name $APPSERVICE_API --resource-group $RESOURCE_GROUP

# View logs
az webapp log tail --name $APPSERVICE_API --resource-group $RESOURCE_GROUP
```

### SQL Server
```bash
# Connect from local
sqlcmd -S localhost,1433 -U sa -P YourPassword123!

# Run migration
sqlcmd -S localhost,1433 -U sa -P YourPassword123! -i migration.sql

# Backup database
BACKUP DATABASE InsureXDb TO DISK = '/var/opt/mssql/backup/InsureXDb.bak'
```

---

## TROUBLESHOOTING

### API won't start
```bash
# Check logs
docker logs insurex-api

# Check connection string
echo $ConnectionStrings__DefaultConnection

# Test database
sqlcmd -S sqlserver -U sa -P YourPassword123! -Q "SELECT 1"
```

### Database migration fails
```bash
# Rollback last migration
dotnet ef migrations remove

# Check migration status
dotnet ef migrations list

# Generate script to review
dotnet ef migrations script --idempotent -o migration.sql
```

### Frontend can't reach API
```bash
# Check CORS in API
# Check API_URL in .env
# Test API endpoint from browser
curl http://localhost:5000/health
```

---

**Document Version:** 1.0  
**Last Updated:** January 2026
