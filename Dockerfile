FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy API and its dependencies only
COPY TaskManagementSystem.API/TaskManagementSystem.API.csproj ./TaskManagementSystem.API/
COPY TaskManagementSystem.Core/TaskManagementSystem.Core.csproj ./TaskManagementSystem.Core/
COPY TaskManagementSystem.Infrastructure/TaskManagementSystem.Infrastructure.csproj ./TaskManagementSystem.Infrastructure/

# Restore as distinct layers
RUN dotnet restore ./TaskManagementSystem.API/TaskManagementSystem.API.csproj

# Copy everything else and build
COPY TaskManagementSystem.API/. ./TaskManagementSystem.API/
COPY TaskManagementSystem.Core/. ./TaskManagementSystem.Core/
COPY TaskManagementSystem.Infrastructure/. ./TaskManagementSystem.Infrastructure/
RUN dotnet publish TaskManagementSystem.API/TaskManagementSystem.API.csproj -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Configure networking and environment
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production
# Disable HTTPS redirection in production
ENV ASPNETCORE_HTTPS_PORT=
ENV ASPNETCORE_HTTP_PORTS=${PORT:-8080}
EXPOSE ${PORT:-8080}

# Ensure data directory exists
RUN mkdir -p /app/Data

# Start the application with proper logging
ENTRYPOINT ["dotnet", "TaskManagementSystem.API.dll", "--urls", "http://+:${PORT:-8080}", "--verbose"]