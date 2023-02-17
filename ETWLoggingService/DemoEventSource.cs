using System.Diagnostics.Tracing;

namespace ETWLoggingService
{
    [EventSource(Name = "DemoEventSource")]
    internal class DemoEventSource : EventSource
    {
        public static DemoEventSource Log { get; } = new DemoEventSource();

        [Event(1, Message = "The current time as ticks is {0}", Level = EventLevel.Informational)]
        public void LogData(long ticks) => WriteEvent(1, ticks);
    }
}
