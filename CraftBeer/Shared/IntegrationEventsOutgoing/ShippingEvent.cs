using Shared.DTOs;

namespace Shared.IntegrationEventsOutgoing;

public record ShippingEvent : IntegrationEventOutgoing
{
    public List<StockItemDto> Items { get; init; } = new();
    //TODO: Again, what is Qty for here?
    public int Quantity { get; init; }
}
