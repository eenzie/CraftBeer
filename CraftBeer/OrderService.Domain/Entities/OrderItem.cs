namespace OrderService.Domain.Entities;

public record OrderItem(StockType ItemType, int Quantity)
{
    public OrderItem() : this(default, 1)
    {
    }
}
