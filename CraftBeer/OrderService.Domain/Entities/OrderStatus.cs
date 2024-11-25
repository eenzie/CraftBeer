using System.Text.Json.Serialization;

namespace OrderService.Domain.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    OrderReceived = 0,
    CheckingStock = 1,
    SufficientStock = 2,
    InsufficientStock = 3,
    CheckingPayment = 4,
    PaymentSuccess = 5,
    PaymentFailed = 6,
    Shipping = 7,
    ShippingFailed = 8,
    Error = 9,
}
