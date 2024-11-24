namespace Shared.DTOs;

public record OrderItemDto(string Id, string StockType, int Quantity, double Total);