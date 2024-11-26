using Dapr.Workflow;
using OrderService.Domain.Entities;
using Shared.DTOs;
namespace OrderService.Application.Activities;

public class OrderCreationActivity : WorkflowActivity<OrderDto, object?>
{
    public override Task<object?> RunAsync(WorkflowActivityContext context, OrderDto input)
    {
        var orderItems = new List<OrderItem>();

        foreach (var itemDto in input.OrderItemsDto)
        {
            var orderItem = OrderItem.FromDto(itemDto);
            orderItems.Add(orderItem);
        }
        var customer = Customer.FromDto(input.CustomerDto);

        var orderStatus = (OrderStatus)(int)input.StatusDto;
        try
        {
            var order = Order.Create(input.OrderId,
                             orderItems,
                             input.OrderDate,
                             customer,
                             orderStatus);

            return Task.FromResult<object?>(null);  // Understreger at null er en bevidst return type
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception in Order.Create:");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            throw; // Allows workflow retry
        }

        //return null;  //Virker ikke her, men fint i de andre activities.
        // Smider en "object not set to an instance of an object" exception.
        // Ikke kompatibel med async Task?
    }
}
