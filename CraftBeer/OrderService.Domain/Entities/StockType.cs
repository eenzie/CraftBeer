namespace OrderService.Domain.Entities;

public record StockType(string Name, double Price)
{
    // Static list to store predefined stock types
    public static readonly StockType Ale = new("Ale", 3);
    public static readonly StockType PaleAle = new("PaleAle", 5);
    public static readonly StockType Stout = new("Stout", 7);
    public static readonly StockType Lager = new("Lager", 8);
    public static readonly StockType Pilsner = new("Pilsner", 12);

    // Collects of all stock types for iteration
    public static readonly IReadOnlyList<StockType> AllStockTypes = new List<StockType>
        {
            Ale,
            PaleAle,
            Stout,
            Lager,
            Pilsner
        };

    /// <summary>
    /// Finds a StockType by Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>StockType</returns>
    public static StockType? FromName(string name)
    {
        return AllStockTypes.FirstOrDefault(type => type.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
