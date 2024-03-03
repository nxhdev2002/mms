#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/prod.Web.Host/prod.Web.Host.csproj", "src/prod.Web.Host/"]
COPY ["src/prod.Web.Core/prod.Web.Core.csproj", "src/prod.Web.Core/"]
COPY ["src/prod.Application/prod.Application.csproj", "src/prod.Application/"]
COPY ["src/prod.Application.Shared/prod.Application.Shared.csproj", "src/prod.Application.Shared/"]
COPY ["src/prod.Core.Shared/prod.Core.Shared.csproj", "src/prod.Core.Shared/"]
COPY ["src/prod.Core/prod.Core.csproj", "src/prod.Core/"]
COPY ["src/prod.EntityFrameworkCore/prod.EntityFrameworkCore.csproj", "src/prod.EntityFrameworkCore/"]
COPY ["src/prod.GraphQL/prod.GraphQL.csproj", "src/prod.GraphQL/"]
RUN dotnet restore "src/prod.Web.Host/prod.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/prod.Web.Host"
RUN dotnet build "prod.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "prod.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "prod.Web.Host.dll"]
