namespace CraftBeer.OrderService.Domain
{
	public record OrderItem(ItemType ItemType, int Quantity)
	{
		public OrderItem() : this(default, 1)
		{
		}
	}
}
