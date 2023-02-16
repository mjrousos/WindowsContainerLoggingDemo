# Could build the Docker file from .NET base images, but it's easy to just build this
# on the existing image from the other Dockerfile.

FROM mjrousos/windowscontainer-logging-demo:latest

# Copy LogMonitor and config file
COPY ETWLoggingService/LogMonitor/LogMonitor.exe ETWLoggingService/LogMonitor/LogMonitorConfig.json /LogMonitor/

# https://github.com/containerd/containerd/issues/5067
# SHELL ["C:\\LogMonitor\\LogMonitor.exe", "cmd", "/S", "/C"]
# # Re-define entrypoint to use shell form
# ENTRYPOINT dotnet ETWLoggingService.dll

ENTRYPOINT [ "C:\\LogMonitor\\LogMonitor.exe", "dotnet", "ETWLoggingService.dll" ]