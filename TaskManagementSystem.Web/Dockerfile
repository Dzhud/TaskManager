FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the Web project and its dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf

# Configure networking
ENV PORT=8080
EXPOSE 8080

# Create start script
RUN echo '#!/bin/sh\n\
sed -i "s/8080/$PORT/g" /etc/nginx/nginx.conf\n\
nginx -g "daemon off;"' > /start.sh && \
chmod +x /start.sh

# Start nginx
CMD ["/start.sh"]