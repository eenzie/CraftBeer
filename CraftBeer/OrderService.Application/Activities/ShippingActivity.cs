using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.IntegrationEventsOutgoing;
using Shared.Queues;

namespace OrderService.Application.Activities;

public class ShippingActivity : WorkflowActivity<OrderItemDto, object?>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationActivity> _logger;

    public ShippingActivity(DaprClient daprClient, ILogger<NotificationActivity> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public override async Task<object?> RunAsync(WorkflowActivityContext context, OrderItemDto input)
    {
        _logger.LogInformation($"About to publish: {input}");

        var shippingRequestMessage = new ShippingEvent { CorrelationId = context.InstanceId };

        await _daprClient.PublishEventAsync(WarehouseChannel.Channel,
                                            WarehouseChannel.Topics.Shipment,
                                            shippingRequestMessage);

        return null;
    }
}
