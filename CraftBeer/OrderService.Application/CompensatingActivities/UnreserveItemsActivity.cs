using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using OrderService.Application.Activities;
using Shared.DTOs;
using Shared.FailedIntegrationEventsIncoming;
using Shared.Queues;

namespace OrderService.Application.CompensatingActivities;

public class UnreserveItemsActivity : WorkflowActivity<OrderItemDto, object?>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationActivity> _logger;

    public UnreserveItemsActivity(DaprClient daprClient, ILogger<NotificationActivity> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public override async Task<object?> RunAsync(WorkflowActivityContext context, OrderItemDto input)
    {
        _logger.LogInformation($"Unreserving items for OrderId: {context.InstanceId}");

        var unreserveItemsEvent = new ReservationFailedEvent
        {
            CorrelationId = context.InstanceId,
            Items = new List<OrderItemDto> { input },
            Reason = "Reservation failed due to insufficient inventory"
        };

        await _daprClient.PublishEventAsync(WarehouseChannel.Channel,
                                            WarehouseChannel.Topics.ReservationFailed,
                                            unreserveItemsEvent);

        _logger.LogInformation($"Unreserve request published for OrderId: {context.InstanceId}");

        return null;
    }
}
