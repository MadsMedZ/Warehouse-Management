using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Models;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase {
    private static readonly List<Product> Products = [
        new Product {
            Id = 1,
            Name = "Lego",
            Location = "123-01-01"
        },
        new Product {
            Id = 2,
            Name = "Game",
            Location = "111-02-03"
        },
        new Product {
            Id = 3,
            Name = "Perfume",
            Location = "205-03-01"
        }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts() {
        return Ok(Products);
    }
}