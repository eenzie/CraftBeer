using Shared.DTOs;

namespace Shared.IntegrationEventsOutgoing;

public record ReservationEvent : IntegrationEventOutgoing
{
    public List<StockItemDto> Items { get; init; } = new();
    //TODO: What is Qty used for here? The qty is in the Stock Item List already
    public int Quantity { get; init; }
}
