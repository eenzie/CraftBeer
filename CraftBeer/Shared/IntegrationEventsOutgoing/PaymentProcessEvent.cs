namespace Shared.IntegrationEventsOutgoing;

public record PaymentProcessEvent : IntegrationEventOutgoing
{
    public decimal Amount { get; init; }
}
