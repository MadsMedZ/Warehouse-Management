using System.ComponentModel.DataAnnotations;

namespace Warehouse.Api.Requests;

public class CreateProductRequests {
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(10)]
    public string Location { get; set; } = "";

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}