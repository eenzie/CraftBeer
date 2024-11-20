using OrderService.Application.Helpers.Commands.DTOs;

namespace OrderService.Application.Helpers.Commands;

public interface IOrderCommand
{
    Guid CreateOrder(CreateOrderDto createOrderDto);
    Guid UpdateOrderDto(UpdateOrderDto updateOrderDto);
    Guid DeleteOrder(Guid id);

}
