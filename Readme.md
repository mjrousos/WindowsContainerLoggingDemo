# Windows Container Logging Sample

This simple app demonstrates techniques for capturing ETW and event log diagnostics from Windows containers.

## Building and running

To run locally, simply build and execute the ETWLoggingService project.

1. ETW events are only visible when subscribed to. They can be seen by profiling with dotnet-trace and specifying the event source name: `dotnet-trace collect --providers DemoEventSource -p <PID>`
1. Event log events will appear in the machine's application event log.

To build and run in a container, first build the docker image by building the included Dockerfile. Note that the image is Server Core based (rather than Nanoserer) even though it's a .NET 7 solution. This is because Server Core is required for LogMonitor to monitor ETW events.

`docker build -t windowscontainer-logging-demo:latest -f .\ETWLoggingService\Dockerfile .`

The container can be run locally by running `docker run windowscontainer-logging-demo:latest`

To view diagnostics, it is possible to connect to the container and, again, use dotnet-trace.

1. `docker exec -it <Container ID> cmd`
1. `curl https://aka.ms/dotnet-trace/win-x64 -L -o dotnet-trace.exe`
1. `dotnet-trace ps`
1. `dotnet-trace collect --providers DemoEventSource -p <PID>`

The trace can then be copied out of the container and analyzed.

1. `docker cp 2be:C:\app\dotnet.exe_20230215_104441.nettrace .\dotnet.exe_20230215_104441.nettrace`
    1. Note that the container must be stopped when using Hyper-V containers (Windows 11) before files can be copied from it.

To view diagnostics in Kubernetes, the [recommended solution](https://kubernetes.io/docs/concepts/windows/user-guide) is [LogMonitor](https://github.com/microsoft/windows-container-tools/tree/main/LogMonitor).

To view diagnostics with LogMonitor, use the Dockerfile.LogMontior Dockerfile to build the container.

1. `docker build -t logmonitor-demo:latest -f .\ETWLoggingService\LogMonitor.Dockerfile .`
1. `docker run --rm logmonitor-demo:latest`

Finally, to view logs from Kubernetes, the LogMonitor-enabled container can be deployed to the cluster.

1. `kubectl apply -f logmonitor-demo.yaml`
