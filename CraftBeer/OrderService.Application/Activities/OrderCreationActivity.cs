using Dapr.Workflow;
using OrderService.Domain.Entities;
using Shared.DTOs;
namespace OrderService.Application.Activities;

public class OrderCreationActivity : WorkflowActivity<OrderDto, object?>
{
    public override Task<object?> RunAsync(WorkflowActivityContext context, OrderDto input)
    {
        var orderItems = input.OrderItemsDto.Select(OrderItem.FromDto).ToList();

        var customer = Customer.FromDto(input.CustomerDto);

        var orderStatus = (OrderStatus)input.StatusDto;

        var order = Order.Create(input.OrderId,
                                 orderItems,
                                 input.OrderDate,
                                 customer,
                                 orderStatus);

        return null;
    }
}
