namespace CraftBeer.OrderService.Domain
{
	public record Order(
	string OrderId,
	OrderItem[] OrderItems,
	DateTime OrderDate,
	Customer CustomerDto,
	double TotalAmount,
	OrderStatus Status = OrderStatus.Received)
	{
		public string ShortId => OrderId.Substring(0, 8);

		public double Amount { get; set; }
	}
}
