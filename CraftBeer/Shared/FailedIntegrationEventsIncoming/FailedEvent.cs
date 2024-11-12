using Shared.IntegrationEventsIncoming;

namespace Shared.FailedIntegrationEventsIncoming
{
    public abstract record FailedEvent : IntegrationEventIncoming
    {
        public string Reason { get; init; } = string.Empty;
    }
}
