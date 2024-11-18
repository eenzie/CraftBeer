using OrderService.Domain.Entities;

namespace OrderService.Application.Helpers.Commands.DTOs
{
    public class CreateOrderDto
    {
        public string Id { get; init; }
        public double Amount { get; set; }
        public string OrderId { get; }
        public List<OrderItem> OrderItems { get; }
        public DateTime OrderDate { get; }
        public Customer Customer { get; }
        public double Total { get; }
        public OrderStatus Status { get; }
    }
}
