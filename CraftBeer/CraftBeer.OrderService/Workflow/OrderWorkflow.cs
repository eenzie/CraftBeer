using CraftBeer.OrderService.Domain;
using Dapr.Workflow;

namespace CraftBeer.OrderService.Workflow
{
	public class OrderWorkflow : Workflow<Order, OrderResult>
	{
		public override Task<OrderResult> RunAsync(WorkflowContext context, Order input)
		{
			// TODO: Implement workflow
			throw new NotImplementedException();
		}
	}
}
