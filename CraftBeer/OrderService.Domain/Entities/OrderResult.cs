using Shared.DTOs;
namespace OrderService.Domain.Entities;

public record OrderResult(OrderStatusDto StatusDto, OrderDto OrderDto, string? Message = null);
