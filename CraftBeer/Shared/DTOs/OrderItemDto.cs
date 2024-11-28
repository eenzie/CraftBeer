namespace Shared.DTOs;

public record OrderItemDto(string Id, string StockTypeDto, int Quantity, double Total);