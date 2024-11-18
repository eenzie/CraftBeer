using System.Text.Json.Serialization;

namespace Shared.DTOs;

public class OrderDto
{
    [JsonPropertyName("OrderId")]
    public string OrderId { get; set; }

    [JsonPropertyName("OrderItemsDto")]
    public List<OrderItemDto> OrderItemsDto { get; set; }

    [JsonPropertyName("OrderDate")]
    public DateTime OrderDate { get; set; }

    [JsonPropertyName("CustomerDto")]
    public CustomerDto CustomerDto { get; set; }

    [JsonPropertyName("Total")]
    public double Total { get; set; }

    [JsonPropertyName("StatusDto")]
    public OrderStatusDto StatusDto { get; set; }

    //[JsonConstructor]
    //public OrderDto(string orderId,
    //                List<OrderItemDto> orderItems,
    //                DateTime orderDate,
    //                CustomerDto customer,
    //                double total,
    //                OrderStatusDto status = OrderStatusDto.OrderReceived)
    //{
    //    OrderId = orderId;
    //    OrderItemsDto = orderItems;
    //    OrderDate = orderDate;
    //    CustomerDto = customer;
    //    Total = total;
    //    StatusDto = status;
    //}
}