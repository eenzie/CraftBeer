using Shared.DTOs;
namespace OrderService.Domain.Entities;

public record Notification(string Message, OrderDto OrderDto);
