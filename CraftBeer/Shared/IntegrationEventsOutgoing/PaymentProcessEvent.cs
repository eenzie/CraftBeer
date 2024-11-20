namespace Shared.IntegrationEventsOutgoing;

public record PaymentProcessEvent : IntegrationEventOutgoing
{
    public double Amount { get; init; }
}
