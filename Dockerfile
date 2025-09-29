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

# Configure networking
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE ${PORT:-8080}

# Start the application
ENTRYPOINT ["dotnet", "TaskManagementSystem.API.dll"]