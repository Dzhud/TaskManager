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

# Install gettext for envsubst
RUN apk add --no-cache gettext

# Configure networking
ENV PORT=80
EXPOSE ${PORT}

# Create a script to start nginx
RUN echo '#!/bin/sh\n\
PORT="${PORT:-80}"\n\
envsubst "\\$PORT" < /etc/nginx/nginx.conf > /etc/nginx/nginx.conf.tmp\n\
mv /etc/nginx/nginx.conf.tmp /etc/nginx/nginx.conf\n\
nginx -g "daemon off;"' > /docker-entrypoint.sh && \
chmod +x /docker-entrypoint.sh

# Start nginx
ENTRYPOINT ["/docker-entrypoint.sh"]