FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the Web project and its dependencies
COPY TaskManagementSystem.Web/TaskManagementSystem.Web.csproj ./TaskManagementSystem.Web/

# Restore as distinct layers
RUN dotnet restore ./TaskManagementSystem.Web/TaskManagementSystem.Web.csproj

# Copy everything else and build
COPY TaskManagementSystem.Web/. ./TaskManagementSystem.Web/
RUN dotnet publish TaskManagementSystem.Web/TaskManagementSystem.Web.csproj -c Release -o /app

# Build runtime image
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/wwwroot .
COPY TaskManagementSystem.Web/nginx.conf /etc/nginx/nginx.conf

# Configure networking
ENV PORT=80
EXPOSE ${PORT}

# Start nginx
CMD sed -i -e 's/$PORT/'"$PORT"'/g' /etc/nginx/nginx.conf && nginx -g 'daemon off;'