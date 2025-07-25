#!/bin/bash

# Build and start all services
docker compose up postgres -d
docker compose up keycloak -d
docker compose up api-gateway -d --build