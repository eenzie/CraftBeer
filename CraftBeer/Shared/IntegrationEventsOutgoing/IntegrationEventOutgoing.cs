namespace Shared.IntegrationEventsOutgoing;

public record IntegrationEventOutgoing
{
    public string CorrelationId { get; init; } = string.Empty;
}