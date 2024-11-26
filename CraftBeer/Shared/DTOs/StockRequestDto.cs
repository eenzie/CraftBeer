namespace Shared.DTOs;

// TODO: Bliver ikke brugt, da laver en simplificering af OrderDto i OrderWorkflow
public record StockRequestDto(List<OrderItemDto> ItemsRequested);