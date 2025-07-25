FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Services/UserService/TodoApp.UserService.csproj", "src/Services/UserService/"]
COPY ["src/Shared/TodoApp.Shared.Contracts/TodoApp.Shared.Contracts.csproj", "src/Shared/TodoApp.Shared.Contracts/"]
RUN dotnet restore "src/Services/UserService/TodoApp.UserService.csproj"
COPY . .
WORKDIR "/src/src/Services/UserService"
RUN dotnet build "TodoApp.UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoApp.UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApp.UserService.dll"]
