using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.IntegrationEventsOutgoing;
using Shared.Queues;

namespace OrderService.Application.Activities;

public class ReservationActivity : WorkflowActivity<List<OrderItemDto>, object?>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationActivity> _logger;

    public ReservationActivity(DaprClient daprClient, ILogger<NotificationActivity> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public override async Task<object?> RunAsync(WorkflowActivityContext context, List<OrderItemDto> input)
    {
        foreach (var item in input)
        {
            _logger.LogInformation($"About to publish reservation of {input.Count} item(s): " +
                                    $"Id={item.Id}, " +
                                    $"StockType={item.StockTypeDto}, " +
                                    $"Quantity={item.Quantity}, " +
                                    $"Total={item.Total}");
        }

        //Call application reservation logic her

        var reservationRequestMessage = new ReservationEvent { CorrelationId = context.InstanceId };

        await _daprClient.PublishEventAsync(WarehouseChannel.Channel,
                                            WarehouseChannel.Topics.Reservation,
                                            reservationRequestMessage);

        //return null;  //Begge virker her...?!
        return Task.FromResult<object?>(null);
    }
}
