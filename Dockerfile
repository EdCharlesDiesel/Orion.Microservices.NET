FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG PROJECT_PATH
WORKDIR /src
COPY . .
RUN dotnet restore "$PROJECT_PATH"
RUN dotnet publish "$PROJECT_PATH" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
ARG APP_DLL
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["sh", "-c", "dotnet ${APP_DLL}"]