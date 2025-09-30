FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_URLS=http://+:9001
USER $APP_UID
WORKDIR /app
EXPOSE 9001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Orion.Services.UserProfile/Orion.Services.UserProfile.API/Orion.Services.UserProfile.API.csproj", "src/Services/Orion.Services.UserProfile/Orion.Services.UserProfile.API/"]
COPY ["src/Core/Orion.Core.TradingEconomics/Orion.Core.TradingEconomics.csproj", "src/Core/Orion.Core.TradingEconomics/"]
COPY ["src/Core/Orion.Core.UserProfile/Orion.Core.UserProfile.csproj", "src/Core/Orion.Core.UserProfile/"]
RUN dotnet restore "src/Services/Orion.Services.UserProfile/Orion.Services.UserProfile.API/Orion.Services.UserProfile.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Orion.Services.UserProfile/Orion.Services.UserProfile.API"
RUN dotnet build "./Orion.Services.UserProfile.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Orion.Services.UserProfile.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orion.Services.UserProfile.API.dll"]
