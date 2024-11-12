namespace OrderService.Domain.Entities;

public record OrderResult(OrderStatus Status, Order Order, string? Message = null);
