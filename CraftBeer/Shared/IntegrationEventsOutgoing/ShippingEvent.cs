using Shared.DTOs;

namespace Shared.IntegrationEventsOutgoing;

public record ShippingEvent : IntegrationEventOutgoing
{
    public List<StockItemDto> Items { get; init; } = new();
}
