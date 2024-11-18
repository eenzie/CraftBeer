using OrderService.Domain.Entities;

namespace OrderService.Application.Helpers.Commands.DTOs
{
    public class UpdateOrderDto
    {
        public Guid Id { get; init; }
        public double Amount { get; set; }
        public string OrderId { get; }
        public OrderItem[] OrderItems { get; }
        public DateTime OrderDate { get; }
        public Customer Customer { get; }
        public double TotalAmount { get; }
        public OrderStatus Status { get; }
    }
}
