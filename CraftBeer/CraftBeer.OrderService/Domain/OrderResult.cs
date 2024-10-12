namespace CraftBeer.OrderService.Domain
{
	public record OrderResult(OrderStatus Status, Order Order, string? Message = null);
}
