using OrderService.Application.Helpers.Commands.DTOs;

namespace OrderService.Application.Helpers.Commands;

//Not in use.
//Question is whether it is necessary to have separate Command-type interfaces,
//when Activities have effectively replaced Commands
public interface IOrderCommand
{
    Guid CreateOrder(CreateOrderDto createOrderDto);
    Guid UpdateOrderDto(UpdateOrderDto updateOrderDto);
    Guid DeleteOrder(Guid id);

}
