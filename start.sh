#!/bin/bash
cd TaskManagementSystem.API
# Use PORT from environment or default to 8080
export ASPNETCORE_URLS="http://+:${PORT:-8080}"
dotnet TaskManagementSystem.API.dll