namespace Warehouse.Api.Models;

public class Product {
    public int Id { get; set;}  // SKU
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    // public int Amount { get; set; }
}