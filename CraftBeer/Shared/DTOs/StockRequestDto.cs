namespace Shared.DTOs;

public record StockRequestDto(List<OrderItemDto> ItemsRequested);