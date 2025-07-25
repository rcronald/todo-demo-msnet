FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/ApiGateway/TodoApp.ApiGateway.csproj", "src/ApiGateway/"]
RUN dotnet restore "src/ApiGateway/TodoApp.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/ApiGateway"
RUN dotnet build "TodoApp.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoApp.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApp.ApiGateway.dll"]
