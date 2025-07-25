FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Services/TaskService/TodoApp.TaskService.csproj", "src/Services/TaskService/"]
COPY ["src/Shared/TodoApp.Shared.Contracts/TodoApp.Shared.Contracts.csproj", "src/Shared/TodoApp.Shared.Contracts/"]
RUN dotnet restore "src/Services/TaskService/TodoApp.TaskService.csproj"
COPY . .
WORKDIR "/src/src/Services/TaskService"
RUN dotnet build "TodoApp.TaskService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoApp.TaskService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApp.TaskService.dll"]
