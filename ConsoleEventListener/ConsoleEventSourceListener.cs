using System.Diagnostics.Tracing;

namespace ConsoleEventListener
{
    internal class ConsoleEventSourceListener : EventListener
    {
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData.Payload is not null)
            {
                Console.WriteLine($"[{eventData.EventName}] {string.Join(", ", eventData.Payload)}");
            }
        }
    }
}
