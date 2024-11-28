﻿using OrderService.Domain.Entities;

namespace OrderService.Application.Helpers.Commands.DTOs
{
    //Not in use:
    //Consideration on whether it is necessary to have separate Command-type DTOs,
    //when Activities have effectively replaced Commands. 
    //It is a question of complexity and validation
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
