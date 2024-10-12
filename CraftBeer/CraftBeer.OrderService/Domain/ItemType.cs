using System.Text.Json.Serialization;

namespace CraftBeer.OrderService.Domain
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ItemType
	{
		Computer = 1,
		Monitor = 2,
		Keyboard = 3,
		Mouse = 4
	}
}
