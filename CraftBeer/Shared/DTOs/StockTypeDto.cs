using System.Text.Json.Serialization;

namespace Shared.DTOs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StockTypeDto
{
    CraftBeer1 = 1,
    CraftBeer2 = 2,
    CraftBeer3 = 3,
    CraftBeer4 = 4
}
