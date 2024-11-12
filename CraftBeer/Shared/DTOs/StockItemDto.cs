namespace Shared.DTOs;

public record StockItemDto
{
    public string StockType { get; init; } = String.Empty;
    public int Quantity { get; init; }
}
