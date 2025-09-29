FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY TaskManagementSystem.API/*.csproj ./TaskManagementSystem.API/
COPY TaskManagementSystem.Core/*.csproj ./TaskManagementSystem.Core/
COPY TaskManagementSystem.Infrastructure/*.csproj ./TaskManagementSystem.Infrastructure/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
COPY start.sh .
RUN chmod +x start.sh

# Configure networking
ENV ASPNETCORE_URLS=http://+:8080
ENV PORT=8080
EXPOSE 8080

# Use shell form to allow environment variable substitution
ENTRYPOINT ./start.sh