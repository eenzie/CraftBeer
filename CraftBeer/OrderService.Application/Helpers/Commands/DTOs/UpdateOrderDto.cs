using OrderService.Domain.Entities;

namespace OrderService.Application.Helpers.Commands.DTOs
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public string OrderId { get; set; }
        public OrderItem[] OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
