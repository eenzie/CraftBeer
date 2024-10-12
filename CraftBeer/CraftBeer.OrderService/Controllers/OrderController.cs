using CraftBeer.OrderService.Domain;
using CraftBeer.OrderService.Workflow;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace CraftBeer.OrderService.Controllers
{
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

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Order order)
		{
			var instanceId = Guid.NewGuid().ToString();
			var workflowComponentName =
				"dapr";
			var workflowName = nameof(OrderWorkflow);

			var startResponse =
				await _daprClient.StartWorkflowAsync(workflowComponentName, workflowName, instanceId, order);
			return Ok(startResponse);
		}
	}
}
