using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.IntegrationEventsOutgoing;
using Shared.Queues;

namespace OrderService.Application.Activities;

public class ReservationActivity : WorkflowActivity<OrderItemDto, object?>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationActivity> _logger;

    public ReservationActivity(DaprClient daprClient, ILogger<NotificationActivity> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public override async Task<object?> RunAsync(WorkflowActivityContext context, OrderItemDto input)
    {
        //TODO: Refine the message output, as currently empty
        _logger.LogInformation($"About to publish: {input}");

        //TODO: Add reservation logic

        var reservationRequestMessage = new ReservationEvent { CorrelationId = context.InstanceId };

        await _daprClient.PublishEventAsync(WarehouseChannel.Channel,
                                            WarehouseChannel.Topics.Reservation,
                                            reservationRequestMessage);

        //TODO: Change to: return Task.FromResult<object?>(null); ?
        return null;
    }
}
