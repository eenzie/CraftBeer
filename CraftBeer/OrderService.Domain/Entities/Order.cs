namespace OrderService.Domain.Entities;

public class Order
{
    public string OrderId { get; init; }
    public List<OrderItem> OrderItems { get; private set; }
    public DateTime OrderDate { get; private set; }
    public Customer Customer { get; private set; }
    public double Total { get; private set; }
    public OrderStatus Status { get; private set; }

    public Order(string orderId,
                List<OrderItem> orderItems,
                DateTime orderDate,
                Customer customer,
                double total,
                OrderStatus status = OrderStatus.OrderReceived)
    {
        OrderId = orderId;
        OrderItems = orderItems;
        OrderDate = orderDate;
        Customer = customer;
        Total = total;
        Status = status;
    }

    public static Order Create(string orderId, List<OrderItem> items, DateTime orderDate, Customer customer, OrderStatus status)
    {
        var total = CalculateTotal(items);

        var order = new Order(orderId, items, orderDate, customer, total, status);

        return order;
    }

    private static double CalculateTotal(List<OrderItem> items)
    {
        var itemTotal = items.Sum(x => x.SubTotal);
        return itemTotal;
    }
}
