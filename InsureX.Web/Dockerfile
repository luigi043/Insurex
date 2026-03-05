FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InsureX.Web/InsureX.Web.csproj", "InsureX.Web/"]
COPY ["IAPR_Data/IAPR_Data.csproj", "IAPR_Data/"]
RUN dotnet restore "InsureX.Web/InsureX.Web.csproj"
COPY . .
WORKDIR "/src/InsureX.Web"
RUN dotnet build "InsureX.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InsureX.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsureX.Web.dll"]
