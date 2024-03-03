#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/prod.Migrator/prod.Migrator.csproj", "src/prod.Migrator/"]
COPY ["src/prod.EntityFrameworkCore/prod.EntityFrameworkCore.csproj", "src/prod.EntityFrameworkCore/"]
COPY ["src/prod.Core/prod.Core.csproj", "src/prod.Core/"]
COPY ["src/prod.Core.Shared/prod.Core.Shared.csproj", "src/prod.Core.Shared/"]
RUN dotnet restore "src/prod.Migrator/prod.Migrator.csproj"
COPY . .
WORKDIR "/src/src/prod.Migrator"
RUN dotnet build "prod.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "prod.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "prod.Migrator.dll", "-s"]
