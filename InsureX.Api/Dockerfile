# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["InsureX.Api/InsureX.Api.csproj", "InsureX.Api/"]
COPY ["IAPR_Data/IAPR_Data.csproj", "IAPR_Data/"]
RUN dotnet restore "InsureX.Api/InsureX.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/InsureX.Api"
RUN dotnet build "InsureX.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "InsureX.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsureX.Api.dll"]
