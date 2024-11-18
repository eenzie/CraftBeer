namespace Shared.IntegrationEventsIncoming
{
    public record PaymentResultEvent : IntegrationEventIncoming
    {
        public double Amount { get; init; }
    }
}
