namespace Shared.DTOs;

public class OrderItemDto
{
    public string Id { get; init; }
    public string StockType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Total { get; set; }

    public OrderItemDto(string id, string stockType, int quantity, double total)
    {
        Id = id;
        StockType = stockType;
        Quantity = quantity;
        Total = total;
    }
}


