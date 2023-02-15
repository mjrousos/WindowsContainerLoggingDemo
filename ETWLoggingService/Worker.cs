using System.Diagnostics;

namespace ETWLoggingService;

public class Worker : BackgroundService
{
    internal const string EventLogSourceName = "Demo";
    internal const string EventLogName = "Application";
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
       //     WriteDiagnosticInformationToETW();
            WriteEventToEventLog(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void WriteEventToEventLog(CancellationToken stoppingToken)
    {
        using (var log = new EventLog(EventLogName))
        {   
            log.Source = EventLogSourceName;
            log.WriteEntry($"The time is now {DateTimeOffset.UtcNow}", EventLogEntryType.Information, 0);
        }
        
        _logger.LogInformation("Event log message written");
    }

    // These events can be captured with dotnet-trace
    // Ex: dotnet-trace collect --providers DemoEventSource -p 30892
    private void WriteDiagnosticInformationToETW()
    {
        DemoEventSource.Log.LogData(DateTimeOffset.UtcNow.Ticks);
        _logger.LogInformation("Diagnostic information written to ETW");
    }
}
