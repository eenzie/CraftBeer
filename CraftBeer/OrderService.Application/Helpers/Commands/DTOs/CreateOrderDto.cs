using OrderService.Domain.Entities;

namespace OrderService.Application.Helpers.Commands.DTOs
{
    public class CreateOrderDto
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string OrderId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public double Total { get; set; }
        public OrderStatus Status { get; set; }
    }
}
