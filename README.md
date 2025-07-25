# TODO List Application

A modern TODO list application built with microservices architecture using .NET 9.0, React, PostgreSQL, and Keycloak.

## Technology Stack

- **Backend**: .NET 9.0 (Microservices)
- **Frontend**: React 18
- **Database**: PostgreSQL 15
- **Identity Management**: Keycloak
- **Containerization**: Docker & Docker Compose

## Architecture

The application follows a microservices architecture with:

- **API Gateway**: Routes requests and handles authentication
- **Task Service**: Manages TODO tasks, categories, and tags
- **User Service**: Handles user profile management
- **Frontend**: React SPA with modern UI
- **Identity Server**: Keycloak for authentication and authorization

## Quick Start

### Prerequisites

- Docker Desktop
- Node.js 18+ (for local development)
- .NET 9.0 SDK (for local development)

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Start the application:

```bash
docker-compose up -d
```

### Access Points

- **Frontend Application**: http://localhost:3000
- **API Gateway**: http://localhost:5000
- **Keycloak Admin**: http://localhost:8080 (admin/admin)

### Initial Setup

1. Access Keycloak admin console at http://localhost:8080
2. Create a new realm called `todo-realm`
3. Create a new client called `todo-app`
4. Configure the client for React frontend
5. Create test users or register through the application

## Development

### Local Development Setup

1. **Database**: Start PostgreSQL container
```bash
docker-compose up postgres -d
```

2. **Keycloak**: Start Keycloak container
```bash
docker-compose up keycloak -d
```

3. **Backend Services**: Run .NET services locally
```bash
cd src/Services/TaskService
dotnet run

cd src/Services/UserService
dotnet run

cd src/ApiGateway
dotnet run
```

4. **Frontend**: Run React development server
```bash
cd src/Frontend
npm install
npm start
```

## Project Structure

```
├── src/
│   ├── ApiGateway/           # API Gateway service
│   ├── Services/
│   │   ├── TaskService/      # Task management microservice
│   │   └── UserService/      # User management microservice
│   ├── Frontend/             # React frontend application
│   └── Shared/               # Shared libraries and contracts
├── database/                 # Database initialization scripts
├── docs/                     # Documentation
├── docker-compose.yml        # Container orchestration
└── README.md
```

## Features

### Core Features
- ✅ User registration and authentication
- ✅ Task CRUD operations
- ✅ Task categories (Work, Personal, Shopping, Health, Education)
- ✅ Task tags for flexible organization
- ✅ Due date management
- ✅ Task status management (complete/incomplete)
- ✅ Responsive design

### Technical Features
- ✅ Microservices architecture
- ✅ Domain Driven Design (DDD)
- ✅ Clean Architecture
- ✅ SOLID principles
- ✅ RESTful APIs
- ✅ JWT authentication
- ✅ Docker containerization

## API Documentation

### Authentication Endpoints
- `POST /auth/login` - User login
- `POST /auth/logout` - User logout
- `POST /auth/refresh` - Token refresh

### Task Management Endpoints
- `GET /api/tasks` - Retrieve user tasks
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update existing task
- `DELETE /api/tasks/{id}` - Delete task
- `PATCH /api/tasks/{id}/status` - Update task status

### Category and Tag Endpoints
- `GET /api/categories` - Retrieve available categories
- `GET /api/tags` - Retrieve user's tags
- `POST /api/tags` - Create new tag

