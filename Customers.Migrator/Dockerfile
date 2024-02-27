FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Customers.Migrator/Customers.Migrator.csproj", "Customers.Migrator/"]
COPY ["Shared.Migrator/Shared.Migrator.csproj", "Shared.Migrator/"]
RUN dotnet restore "Customers.Migrator/Customers.Migrator.csproj"
COPY . .
WORKDIR "/src/Customers.Migrator"
RUN dotnet build "Customers.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Customers.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customers.Migrator.dll"]
