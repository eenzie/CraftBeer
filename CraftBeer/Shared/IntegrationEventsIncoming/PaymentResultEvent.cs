namespace Shared.IntegrationEventsIncoming
{
    public record PaymentResultEvent : IntegrationEventIncoming
    {
        public decimal Amount { get; init; }
    }
}
