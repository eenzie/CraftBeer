using Shared.DTOs;

namespace Shared.FailedIntegrationEventsIncoming;

public record ShippingFailedEvent : FailedEvent
{
    public List<StockItemDto> Items { get; init; } = new();
}
