﻿using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.ExternalWorkflows;
using Shared.IntegrationEventsIncoming;
using Shared.Queues;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkflowChannelController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<WorkflowChannelController> _logger;
    private readonly string _workflowComponentName = "dapr";

    public WorkflowChannelController(DaprClient daprClient, ILogger<WorkflowChannelController> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.ReservationResult)]
    [HttpPost("reservationresult")]
    public async Task<IActionResult> ReservationResult([FromBody] ReservationResultEvent reservationResult)
    {
        _logger.LogInformation(
            $"Reservation response received: ID: {reservationResult.CorrelationId}, Status: {reservationResult.Status}");

        try
        {
#pragma warning disable CS0618 // Type or member is obsolete
            await _daprClient.RaiseWorkflowEventAsync(reservationResult.CorrelationId,
                                                      _workflowComponentName,
                                                      ExternalEvents.ReservationEvent,
                                                      reservationResult);
#pragma warning restore CS0618 // Type or member is obsolete

            _logger.LogInformation("Reservation response sent to workflow");
        }
        catch (Dapr.DaprException daprEx)
        {
            _logger.LogError(daprEx,
                $"Failed to raise event {ExternalEvents.ReservationEvent} for workflow {reservationResult.CorrelationId}: {daprEx.InnerException?.Message}");
            return StatusCode(500, "Failed to raise event for workflow instance.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in ReservationResult.");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return Ok();
    }

    [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.PaymentResult)]
    [HttpPost("paymentresult")]
    public async Task<IActionResult> PaymentResult([FromBody] PaymentResultEvent paymentResult)
    {
        _logger.LogInformation(
            $"Payment response received: Id: {paymentResult.CorrelationId}, Amount: {paymentResult.Amount.ToString("C")}, Status: {paymentResult.Status}");

        try
        {
#pragma warning disable CS0618 // Type or member is obsolete
            await _daprClient.RaiseWorkflowEventAsync(paymentResult.CorrelationId,
                                                      _workflowComponentName,
                                                      ExternalEvents.PaymentEvent,
                                                      paymentResult);
#pragma warning restore CS0618 // Type or member is obsolete

            _logger.LogInformation("Payment response send to workflow");

        }
        catch (Dapr.DaprException daprEx)
        {
            _logger.LogError(daprEx,
                $"Failed to raise event {ExternalEvents.PaymentEvent} for workflow {paymentResult.CorrelationId}: {daprEx.InnerException?.Message}");
            return StatusCode(500, "Failed to raise event for workflow instance.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in PaymentResult.");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return Ok();
    }

    [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.ShippingResult)]
    [HttpPost("shippingresult")]
    public async Task<IActionResult> ShippingResult([FromBody] ShippingResultEvent itemsShippedResult)
    {
        _logger.LogInformation(
            $"Shipping response received: Id: {itemsShippedResult.CorrelationId}, Status: {itemsShippedResult.Status}");

        try
        {
#pragma warning disable CS0618 // Type or member is obsolete
            await _daprClient.RaiseWorkflowEventAsync(itemsShippedResult.CorrelationId,
                                                  _workflowComponentName,
                                                  ExternalEvents.ShippingEvent,
                                                  itemsShippedResult);
#pragma warning restore CS0618 // Type or member is obsolete

            _logger.LogInformation("Shipping response send to workflow");
        }
        catch (Dapr.DaprException daprEx)
        {
            _logger.LogError(daprEx,
                $"Failed to raise event {ExternalEvents.ShippingEvent} for workflow {itemsShippedResult.CorrelationId}: {daprEx.InnerException?.Message}");
            return StatusCode(500, "Failed to raise event for workflow instance.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in PaymentResult.");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return Ok();
    }
}
