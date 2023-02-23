# It's important that this image be based on Server Core. 
# Nanoserver does not support LogMonitor reading ETW events.
FROM mcr.microsoft.com/dotnet/runtime:7.0-windowsservercore-ltsc2022 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ETWLoggingService/ETWLoggingService.csproj", "ETWLoggingService/"]
RUN dotnet restore "ETWLoggingService/ETWLoggingService.csproj"
COPY . .
WORKDIR "/src/ETWLoggingService"
RUN dotnet build "ETWLoggingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ETWLoggingService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set up custom event log
RUN ["dotnet", "ETWLoggingService.dll", "-setup"]

ENTRYPOINT ["dotnet", "ETWLoggingService.dll"]