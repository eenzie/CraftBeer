namespace Shared.DTOs;

public record OrderItemDto(StockTypeDto ItemType, int Quantity)
{
    public OrderItemDto() : this(default, 1)
    {
    }
}
