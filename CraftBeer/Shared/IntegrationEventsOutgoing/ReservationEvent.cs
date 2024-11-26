using Shared.DTOs;

namespace Shared.IntegrationEventsOutgoing;

public record ReservationEvent : IntegrationEventOutgoing
{
    public List<StockItemDto> Items { get; init; } = new();
}
