using Shared.DTOs;

namespace OrderService.Domain.Entities;

public class OrderItem
{
    public string Id { get; init; }
    public StockType StockType { get; private set; }
    public int Quantity { get; private set; }
    public double SubTotal { get; private set; }

    private OrderItem(string id, StockType stockType, int quantity, double subTotal)
    {
        Id = id;
        StockType = stockType;
        Quantity = quantity;
        SubTotal = subTotal;
    }

    public static OrderItem Create(string id, StockType type, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        var subTotal = quantity * type.Price;

        var orderItem = new OrderItem(id, type, quantity, subTotal);

        return orderItem;
    }

    public static OrderItem FromDto(OrderItemDto dto)
    {
        // Using the static FromName method to get the matching StockType by Name
        var stockType = StockType.FromName(dto.StockTypeDto);

        if (stockType == null)
        {
            throw new ArgumentException($"Invalid StockType: {dto.StockTypeDto}");
        }

        return new OrderItem(
            id: dto.Id,
            stockType: stockType,
            quantity: dto.Quantity,
            subTotal: dto.Total
        );
    }
}
