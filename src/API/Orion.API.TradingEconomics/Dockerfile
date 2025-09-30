FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_URLS=http://+:9002
USER $APP_UID
WORKDIR /app
EXPOSE 9002

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Orion.Services.TradingEconomics/Orion.Services.TradingEconomics.API/Orion.Services.TradingEconomics.API.csproj", "src/Services/Orion.Services.TradingEconomics/Orion.Services.TradingEconomics.API/"]
COPY ["src/Core/Orion.Core.TradingEconomics/Orion.Core.TradingEconomics.csproj", "src/Core/Orion.Core.TradingEconomics/"]
RUN dotnet restore "src/Services/Orion.Services.TradingEconomics/Orion.Services.TradingEconomics.API/Orion.Services.TradingEconomics.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Orion.Services.TradingEconomics/Orion.Services.TradingEconomics.API"
RUN dotnet build "./Orion.Services.TradingEconomics.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Orion.Services.TradingEconomics.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orion.Services.TradingEconomics.API.dll"]
