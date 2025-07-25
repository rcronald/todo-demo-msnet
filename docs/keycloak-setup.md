# Keycloak Configuration Guide

## Initial Setup

After running `docker-compose up`, you need to configure Keycloak for the TODO application.

### 1. Access Keycloak Admin Console

1. Navigate to http://localhost:8080
2. Click "Administration Console"
3. Login with username: `admin` and password: `admin`

### 2. Create Realm

1. Hover over "Master" in the top-left corner
2. Click "Add realm"
3. Name: `todo-realm`
4. Click "Create"

### 3. Create Client

1. In the left sidebar, click "Clients"
2. Click "Create"
3. Client ID: `todo-app`
4. Client Protocol: `openid-connect`
5. Click "Save"

### 4. Configure Client Settings

1. Access Type: `public`
2. Standard Flow Enabled: `ON`
3. Direct Access Grants Enabled: `ON`
4. Valid Redirect URIs: `http://localhost:3000/*`
5. Web Origins: `http://localhost:3000`
6. Click "Save"

### 5. Create Test User (Optional)

1. In the left sidebar, click "Users"
2. Click "Add user"
3. Username: `testuser`
4. Email: `test@example.com`
5. First Name: `Test`
6. Last Name: `User`
7. Email Verified: `ON`
8. Enabled: `ON`
9. Click "Save"

### 6. Set User Password

1. Go to "Credentials" tab
2. Password: `password123`
3. Temporary: `OFF`
4. Click "Set Password"

## Testing the Setup

1. Navigate to http://localhost:3000
2. You should be redirected to Keycloak login
3. Login with the test user credentials
4. You should be redirected back to the TODO application

## Troubleshooting

### Common Issues

1. **"Invalid redirect URI"**: Make sure the redirect URIs are configured correctly in the client settings
2. **CORS errors**: Ensure Web Origins is set to `http://localhost:3000`
3. **User not found**: The application creates user profiles automatically on first login

### Checking Logs

```bash
# View all logs
docker-compose logs

# View specific service logs
docker-compose logs frontend
docker-compose logs keycloak
docker-compose logs api-gateway
docker-compose logs task-service
```

### Restart Services

```bash
# Restart all services
docker-compose restart

# Restart specific service
docker-compose restart frontend
```

http://localhost:3000/login#iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&state=b548fad6-dfea-4bf7-aad4-c5dbd00f8c76&session_state=e7c8b44c-9764-4d52-b8ba-07558c7ce25e&iss=http%3A%2F%2Flocalhost%3A8080%2Frealms%2Ftodo-realm&code=5e0e9577-502a-4839-bdff-85827f9f71d4.e7c8b44c-9764-4d52-b8ba-07558c7ce25e.5b8d7950-f780-4cb0-83a1-b63963b6a46d