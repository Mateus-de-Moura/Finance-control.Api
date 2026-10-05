FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY finance-control.sln ./
COPY src/finance-control.Api/finance-control.Api.csproj src/finance-control.Api/
COPY src/finance-control.Application/finance-control.Application.csproj src/finance-control.Application/
COPY src/finance-control.Domain/finance-control.Domain.csproj src/finance-control.Domain/
COPY src/finance-control.Infra.Data/finance-control.Infra.Data.csproj src/finance-control.Infra.Data/
COPY src/finance-control.Services/finance-control.Services.csproj src/finance-control.Services/
COPY hangFire/BackgroundService/BackgroundService.csproj hangFire/BackgroundService/
RUN dotnet restore src/finance-control.Api/finance-control.Api.csproj
RUN dotnet restore hangFire/BackgroundService/BackgroundService.csproj

COPY src/ src/
COPY hangFire/ hangFire/
RUN dotnet publish src/finance-control.Api/finance-control.Api.csproj -c Release -o /app/publish --no-restore
RUN dotnet publish hangFire/BackgroundService/BackgroundService.csproj -c Release -o /app/hangfire --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS hangfire
WORKDIR /app
COPY --from=build /app/hangfire .
EXPOSE 8080
ENTRYPOINT ["dotnet", "BackgroundService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS api
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "finance-control.Api.dll"]
