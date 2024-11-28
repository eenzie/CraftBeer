namespace Shared.DTOs;

public record StockItemDto
{
    public string StockType { get; set; }
    public int Quantity { get; set; }
}
