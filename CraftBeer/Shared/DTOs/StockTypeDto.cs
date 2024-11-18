namespace Shared.DTOs;

public record StockTypeDto(string Name, double Price)
{
    // Static list to store predefined stock types
    public static readonly StockTypeDto Ale = new("Ale", 3);
    public static readonly StockTypeDto PaleAle = new("PaleAle", 5);
    public static readonly StockTypeDto Stout = new("Stout", 7);
    public static readonly StockTypeDto Lager = new("Lager", 8);
    public static readonly StockTypeDto Pilsner = new("Pilsner", 12);

    //TODO: Remove ReadOnlyList of StockTypeDto?
    //Optional: Collection of all stock types for iteration
    public static readonly IReadOnlyList<StockTypeDto> AllStockTypes = new List<StockTypeDto>
    {
        Ale,
        PaleAle,
        Stout,
        Lager,
        Pilsner
    };
}
