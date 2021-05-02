FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

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
ENTRYPOINT ["dotnet", "DemirorenCase.LoggerWorkerService.dll"]
