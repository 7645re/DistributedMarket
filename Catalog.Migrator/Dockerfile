FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Catalog.Migrator/Catalog.Migrator.csproj", "Catalog.Migrator/"]
RUN dotnet restore "Catalog.Migrator/Catalog.Migrator.csproj"
COPY . .
WORKDIR "/src/Catalog.Migrator"
RUN dotnet build "Catalog.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Catalog.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Migrator.dll"]

ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT