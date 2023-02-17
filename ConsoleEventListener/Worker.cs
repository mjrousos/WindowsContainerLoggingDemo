using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Tracing;
using System.Text;

namespace ConsoleEventListener;

public class Worker : BackgroundService, IDisposable
{
    private const string EventSourceName = "DemoEventSource";

    private readonly ILogger<Worker> _logger;
    private readonly TraceEventSession _traceEventSession;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _traceEventSession = new TraceEventSession("DemoEventSession");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _traceEventSession.Source.Dynamic.AddCallbackForProviderEvents(IsProviderDemoEventSource, LogEvent);
        _traceEventSession.EnableProvider(EventSourceName);
        _traceEventSession.Source.Process();

        return Task.CompletedTask;
    }

    private void LogEvent(TraceEvent traceEvent)
    {
        var formatStringBuilder = new StringBuilder("{ProviderName}/{EventName} [{Level}]: {FormattedMessage}");
        foreach (var payloadName in traceEvent.PayloadNames)
        {
            formatStringBuilder.Append($", {payloadName}={{{payloadName}}}");
        }
        var formatString = formatStringBuilder.ToString();

        if (traceEvent.PayloadNames.Length == 0)
        {
            _logger.LogInformation(formatString, traceEvent.ProviderName, traceEvent.EventName, traceEvent.Level, traceEvent.FormattedMessage);
        }
        else if (traceEvent.PayloadNames.Length == 1)
        {
            _logger.LogInformation(formatString, traceEvent.ProviderName, traceEvent.EventName, traceEvent.Level, traceEvent.FormattedMessage, traceEvent.PayloadValue(0));
        }
        else if (traceEvent.PayloadNames.Length == 2)
        {
            _logger.LogInformation(formatString, traceEvent.ProviderName, traceEvent.EventName, traceEvent.Level, traceEvent.FormattedMessage, traceEvent.PayloadValue(0), traceEvent.PayloadValue(1));
        }
    }

    private EventFilterResponse IsProviderDemoEventSource(string providerName, string eventName) =>
        EventSourceName.Equals(providerName, StringComparison.Ordinal)
        ? EventFilterResponse.AcceptEvent
        : EventFilterResponse.RejectProvider;

    public override void Dispose()
    {
        _traceEventSession?.Dispose();
        base.Dispose();
    }
}
