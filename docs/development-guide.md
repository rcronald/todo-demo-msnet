# Development Guide

## Quick Start

1. **Prerequisites**
   - Docker Desktop installed and running
   - Docker Compose installed

2. **Run the Setup Script**
   ```bash
   ./setup.sh
   ```

3. **Configure Keycloak** (First-time setup only)
   - Follow the instructions in `docs/keycloak-setup.md`

4. **Access the Application**
   - Frontend: http://localhost:3000
   - API Gateway: http://localhost:5000
   - Keycloak: http://localhost:8080

## Development Workflow

### Starting the Application
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

### Development Mode (Optional)

For local development of individual services:

1. **Start Dependencies Only**
   ```bash
   docker-compose up postgres keycloak -d
   ```

2. **Run Backend Services Locally**
   ```bash
   # Task Service
   cd src/Services/TaskService
   dotnet run

   # User Service  
   cd src/Services/UserService
   dotnet run

   # API Gateway
   cd src/ApiGateway
   dotnet run
   ```

3. **Run Frontend Locally**
   ```bash
   cd src/Frontend
   npm install
   npm start
   ```

### Testing

1. **Backend API Testing**
   - Swagger UI available at service endpoints in development mode
   - Use Postman or curl with Bearer token authentication

2. **Frontend Testing**
   ```bash
   cd src/Frontend
   npm test
   ```

### Database Access

PostgreSQL is accessible at `localhost:5432` with:
- Database: `todoapp`
- Username: `todouser`
- Password: `todopass`

## Architecture Overview

### Microservices
- **Task Service**: Manages tasks, categories, and tags
- **User Service**: Handles user profile management
- **API Gateway**: Routes requests and handles authentication

### Frontend
- **React SPA**: Modern single-page application
- **Keycloak Integration**: Secure authentication
- **Responsive Design**: Works on desktop, tablet, and mobile

### Database Schema
- Users table for user profiles
- Tasks table with relationships to categories and tags
- Categories table with predefined categories
- Tags table for user-created tags
- TaskTags junction table for many-to-many relationships

## Troubleshooting

### Common Issues

1. **Services won't start**
   - Check if ports are available (3000, 5000, 8080, 5432)
   - Ensure Docker Desktop is running
   - Try `docker-compose down` then `docker-compose up --build`

2. **Authentication not working**
   - Verify Keycloak configuration
   - Check that the realm and client are properly configured
   - Ensure redirect URIs are correct

3. **API calls failing**
   - Check if all services are running: `docker-compose ps`
   - Verify network connectivity between containers
   - Check service logs: `docker-compose logs [service-name]`

4. **Database connection issues**
   - Ensure PostgreSQL container is running
   - Check connection string configuration
   - Verify database is initialized properly

### Logs

```bash
# View all logs
docker-compose logs

# View specific service logs
docker-compose logs frontend
docker-compose logs task-service
docker-compose logs user-service
docker-compose logs api-gateway
docker-compose logs keycloak
docker-compose logs postgres

# Follow logs in real-time
docker-compose logs -f [service-name]
```

### Rebuilding

```bash
# Rebuild all services
docker-compose up --build

# Rebuild specific service
docker-compose up --build [service-name]

# Force recreate containers
docker-compose up --force-recreate
```

## Contributing

1. Follow the established architecture patterns
2. Maintain separation of concerns between services
3. Use proper error handling and logging
4. Write tests for new features
5. Update documentation when adding new features

## Production Deployment

For production deployment:

1. Update environment variables
2. Use proper TLS certificates
3. Configure production-ready Keycloak
4. Set up monitoring and logging
5. Use production database with proper backups
6. Configure reverse proxy/load balancer



dotnet tool install --global dotnet-ef
dotnet ef database update
dotnet ef migrations add InitialCreate
dotnet ef database update