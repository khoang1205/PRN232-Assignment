# Base image for running the app
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

# Stage for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backgroundWorker/backgroundWorker.csproj", "backgroundWorker/"]
RUN dotnet restore "backgroundWorker/backgroundWorker.csproj"
COPY . .
WORKDIR "/src/backgroundWorker"
RUN dotnet build "backgroundWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage for publishing the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "backgroundWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "backgroundWorker.dll"]
