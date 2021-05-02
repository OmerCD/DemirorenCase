FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 2018

ENV ASPNETCORE_URLS=http://+:2018

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DemirorenCase.LoggerWorkerService/DemirorenCase.LoggerWorkerService.csproj", "DemirorenCase.LoggerWorkerService/"]
RUN dotnet restore "DemirorenCase.LoggerWorkerService/DemirorenCase.LoggerWorkerService.csproj"
COPY . .
WORKDIR "/src/DemirorenCase.LoggerWorkerService"
RUN dotnet build "DemirorenCase.LoggerWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemirorenCase.LoggerWorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemirorenCase.LoggerWorkerService.dll", "--environment=Docker"]
