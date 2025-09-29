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
ENV PORT=80
EXPOSE ${PORT}

# Start nginx with proper port substitution
CMD sh -c "envsubst '\$PORT' < /etc/nginx/nginx.conf > /etc/nginx/nginx.conf.tmp && \
    mv /etc/nginx/nginx.conf.tmp /etc/nginx/nginx.conf && \
    nginx -g 'daemon off;'"