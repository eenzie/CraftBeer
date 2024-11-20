namespace Shared.DTOs;

public record StockItemDto
{
    public string StockType { get; set; } = String.Empty;
    public int Quantity { get; set; }
}
