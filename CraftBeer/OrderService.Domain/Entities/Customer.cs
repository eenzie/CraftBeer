using Shared.DTOs;

namespace OrderService.Domain.Entities;

public record Customer(string Name, string Email)
{
    public static Customer FromDto(CustomerDto dto)
    {
        return new Customer(
            Name: dto.Name,
            Email: dto.Email
        );
    }
}