using System.Text.Json.Serialization;

namespace CraftBeer.OrderService.Domain
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum OrderStatus
	{
		Received = 0,
		CheckingInventory = 1,
		SufficientInventory = 2,
		InsufficientInventory = 3,
		CheckingPayment = 4,
		PaymentSuccess = 5,
		PaymentFailing = 6,
		ShipingItems = 7,
		ShipingItemsFailing = 8,
		Error = 9,
	}
}
