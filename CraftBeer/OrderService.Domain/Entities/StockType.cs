using System.Text.Json.Serialization;

namespace OrderService.Domain.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StockType
{
    CraftBeer1 = 1,
    CraftBeer2 = 2,
    CraftBeer3 = 3,
    CraftBeer4 = 4
}
