namespace Shared.DTOs;

public record OrderDto(
    string OrderId,
    OrderItemDto[] OrderItems,
    DateTime OrderDate,
    CustomerDto CustomerDto,
    OrderStatusDto Status = OrderStatusDto.OrderReceived)
{

    //TODO: Change the GUID formatting
    public string ShortId => OrderId.Substring(0, 8);
}
