FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_URLS=http://+:9008
USER $APP_UID
WORKDIR /app
EXPOSE 9008

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Orion.Services.Chat/Orion.Services.Chat.csproj", "src/Services/Orion.Services.Chat/"]
COPY ["src/Core/Orion.Core.Chat/Orion.Core.Chat.csproj", "src/Core/Orion.Core.Chat/"]
RUN dotnet restore "src/Services/Orion.Services.Chat/Orion.Services.Chat.csproj"
COPY . .
WORKDIR "/src/src/Services/Orion.Services.Chat"
RUN dotnet build "./Orion.Services.Chat.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Orion.Services.Chat.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orion.Services.Chat.dll"]
