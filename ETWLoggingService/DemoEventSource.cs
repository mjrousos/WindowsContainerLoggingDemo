using System.Diagnostics.Tracing;

namespace ETWLoggingService
{
    [EventSource(Name = "DemoEventSource", Guid = "2CA233D9-2F97-4B75-80AB-440F91FB9FD3")]
    internal class DemoEventSource : EventSource
    {
        public static DemoEventSource Log { get; } = new DemoEventSource();

        [Event(1)]
        public void LogData(long ticks) => WriteEvent(1, $"The current time as ticks is {ticks}", ticks);
    }
}
