using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.IntegrationEventsOutgoing;
using Shared.Queues;

namespace OrderService.Application.Activities;

public class ShippingActivity : WorkflowActivity<OrderDto, object?>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationActivity> _logger;

    public ShippingActivity(DaprClient daprClient, ILogger<NotificationActivity> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public override async Task<object?> RunAsync(WorkflowActivityContext context, OrderDto input)
    {
        _logger.LogInformation($"About to publish shipping of order {input.OrderId} ordered {input.OrderDate} by {input.CustomerDto.Name}");

        var shippingRequestMessage = new ShippingEvent { CorrelationId = context.InstanceId };

        await _daprClient.PublishEventAsync(WarehouseChannel.Channel,
                                            WarehouseChannel.Topics.Shipment,
                                            shippingRequestMessage);

        return null;
    }
}
