using ETWLoggingService;
using System.Diagnostics;

if (args.Length > 0 && args[0].Equals("-setup", StringComparison.OrdinalIgnoreCase))
{
    if (!EventLog.SourceExists(Worker.EventLogSourceName))
    {
        EventLog.CreateEventSource(Worker.EventLogSourceName, Worker.EventLogName);
    }
}
else
{
    IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
        })
        .Build();

    host.Run();
}