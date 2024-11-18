using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.FailedIntegrationEventsIncoming;
using Shared.IntegrationEventsIncoming;
using Shared.IntegrationEventsOutgoing;
using Shared.Queues;

namespace WarehouseService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly ILogger<WarehouseController> _logger;
    private readonly DaprClient _daprClient;

    public WarehouseController(ILogger<WarehouseController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.Reservation)]
    [HttpPost("reservations/do")]
    public async Task<IActionResult> Post([FromBody] ReservationEvent reserveItemsRequest)
    {
        _logger.LogInformation($"Inventory request received: {reserveItemsRequest.CorrelationId}");

        //TODO: Add logic to reservice item in db. Maybe remove qty from total amount in stock?

        var itemsReservedResponse = new ReservationResultEvent
        {
            CorrelationId = reserveItemsRequest.CorrelationId,
            Status = ResultStatus.Succeeded
        };

        await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.ReservationResult,
            itemsReservedResponse);

        _logger.LogInformation(
            $"Item reserved: {itemsReservedResponse.CorrelationId}, {itemsReservedResponse.Status}");

        return Ok();
    }

    [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.ReservationFailed)]
    [HttpPost("reservations/undo")]
    public async Task<IActionResult> Post([FromBody] ReservationFailedEvent unreserveItemsRequest)
    {
        _logger.LogInformation($"Unreserve request received: {unreserveItemsRequest.CorrelationId}");

        foreach (var item in unreserveItemsRequest.Items)
        {
            _logger.LogInformation($"Unreserving item: {item.StockType}, Quantity: {item.Quantity}");
            // TODO: Implement logic to undo the reservation here.
            // If reserve item removes qty from inventory, then here we add qty back in
        }

        return Ok();
    }

    [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.Shipment)]
    [HttpPost("shipping/do")]
    public async Task<IActionResult> Post([FromBody] ShippingEvent shipItemsEventRequest)
    {
        _logger.LogInformation($"Payment request received: {shipItemsEventRequest.CorrelationId}");

        var itemsShippedResponse = new ShippingResultEvent
        {
            CorrelationId = shipItemsEventRequest.CorrelationId,
            Status = ResultStatus.Succeeded
        };

        await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.ShippingResult,
            itemsShippedResponse);

        _logger.LogInformation(
            $"Payment processed: {itemsShippedResponse.CorrelationId}, , {itemsShippedResponse.Status}");

        return Ok();
    }
}
