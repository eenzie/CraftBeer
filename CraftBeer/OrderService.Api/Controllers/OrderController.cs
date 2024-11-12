using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Workflows;
using OrderService.Domain.Entities;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly DaprClient _daprClient;

    public OrderController(ILogger<OrderController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        var instanceId = Guid.NewGuid().ToString();
        var workflowComponentName =
            "dapr";
        var workflowName = nameof(OrderWorkflow);

        var startResponse =
            await _daprClient.StartWorkflowAsync(workflowComponentName, workflowName, instanceId, order);

        _logger.LogInformation($"Workflow started with instance ID: {instanceId}");

        return Ok(startResponse);
    }
}
