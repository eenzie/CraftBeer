using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using OrderService.Application.Activities;
using OrderService.Domain.Entities;
using Shared.DTOs;

namespace OrderService.Application.CompensatingActivities;

public class CancelOrderActivity : WorkflowActivity<OrderDto, object?>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationActivity> _logger;

    public CancelOrderActivity(DaprClient daprClient, ILogger<NotificationActivity> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }
    public override Task<object?> RunAsync(WorkflowActivityContext context, OrderDto input)
    {
        Order.Delete(input.OrderId);

        return Task.FromResult<object?>(null);  // Understreger at null er en bevidst return type
    }
}
