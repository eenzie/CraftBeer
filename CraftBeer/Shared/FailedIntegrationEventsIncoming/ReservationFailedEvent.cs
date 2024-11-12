using Shared.DTOs;

namespace Shared.FailedIntegrationEventsIncoming;

public record ReservationFailedEvent : FailedEvent
{
    public List<OrderItemDto> Items { get; init; } = new();
}
