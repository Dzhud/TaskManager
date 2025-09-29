# Task Management System

A simple task management system that allows caseworkers to efficiently manage their tasks.

## Features

- Create, read, update, and delete tasks
- Track task status
- Set due dates/times
- Optional task descriptions
- RESTful API
- User-friendly web interface

## Technologies

- Backend: ASP.NET Core Web API
- Frontend: Blazor WebAssembly
- Database: SQLite with Entity Framework Core
- Documentation: Swagger/OpenAPI
- Deployment: Railway.app with Docker containers
- Server: Nginx (Web Frontend)

## Project Structure

- **TaskManagementSystem.API**: REST API endpoints and configuration
- **TaskManagementSystem.Core**: Domain models and interfaces
- **TaskManagementSystem.Infrastructure**: Data access and repository implementations
- **TaskManagementSystem.Web**: Blazor WebAssembly frontend
- **TaskManagementSystem.Tests**: Unit and integration tests

## API Endpoints

- GET /api/tasks - Get all tasks
- GET /api/tasks/{id} - Get a specific task
- POST /api/tasks - Create a new task
- PUT /api/tasks/{id} - Update an existing task
- DELETE /api/tasks/{id} - Delete a task

## Getting Started

### Production URLs

- Web Application: https://taskmanager-production-ad9d.up.railway.app
- API Endpoint: https://taskmanager-production-6bd0.up.railway.app

### Local Development

1. Clone the repository
2. Navigate to the API project directory: `cd TaskManagementSystem.API`
3. Run the database migrations: `dotnet ef database update`
4. Start the API: `dotnet run`
5. Navigate to the Web project directory: `cd ../TaskManagementSystem.Web`
6. Start the web application: `dotnet run`
7. Open your browser and navigate to `https://localhost:7001`

## Development

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Database Migrations

```bash
cd TaskManagementSystem.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Testing

Run the tests using:

```bash
dotnet test
```

## Deployment

The application is deployed on Railway.app with two services:

### API Service
- Deployed from the main branch
- Uses Dockerfile in the root directory
- Configured with SQLite database
- Health check endpoint at `/health`

### Web Service
- Deployed from the main branch
- Uses Dockerfile in TaskManagementSystem.Web directory
- Served using Nginx
- Configured to communicate with the production API
