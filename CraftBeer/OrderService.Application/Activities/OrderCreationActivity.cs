using Dapr.Workflow;
using OrderService.Domain.Entities;
namespace OrderService.Application.Activities;

//TODO: Set up Create order activity
public class OrderCreationActivity : WorkflowActivity<Order, object?>
{
    //readonly IStateManagementRepository _stateManagement;

    //public CreateOrderActivity(IStateManagementRepository stateManagement)
    //{
    //    _stateManagement = stateManagement;
    //}

    public override async Task<object?> RunAsync(WorkflowActivityContext context, Order order)
    {
        //await _stateManagement.SaveOrderAsync(order);

        return null;
    }
}
