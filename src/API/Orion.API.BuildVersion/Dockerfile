FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/Orion.API.BuildVersion/Orion.API.BuildVersion.csproj", "src/API/Orion.API.BuildVersion/"]
COPY ["src/DataAccess/Orion.DataAccess.Postgres/Orion.DataAccess.Postgres.csproj", "src/DataAccess/Orion.DataAccess.Postgres/"]
COPY ["src/Domain/Orion.Domain/Orion.Domain.csproj", "src/Domain/Orion.Domain/"]
RUN dotnet restore "src/API/Orion.API.BuildVersion/Orion.API.BuildVersion.csproj"
COPY . .
WORKDIR "/src/src/API/Orion.API.BuildVersion"
RUN dotnet build "./Orion.API.BuildVersion.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Orion.API.BuildVersion.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orion.API.BuildVersion.dll"]
