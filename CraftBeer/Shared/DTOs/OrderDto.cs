using System.Text.Json.Serialization;

namespace Shared.DTOs;

public record OrderDto(
    [property: JsonPropertyName("OrderId")] string OrderId,
    [property: JsonPropertyName("OrderItemsDto")] List<OrderItemDto> OrderItemsDto,
    [property: JsonPropertyName("OrderDate")] DateTime OrderDate,
    [property: JsonPropertyName("CustomerDto")] CustomerDto CustomerDto,
    [property: JsonPropertyName("Total")] double Total,
    [property: JsonPropertyName("StatusDto")] OrderStatusDto StatusDto
);