namespace Shared.FailedIntegrationEventsIncoming
{
    public record PaymentFailedEvent : FailedEvent
    {
        public double Amount { get; init; }
    }
}
