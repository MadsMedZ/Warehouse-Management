using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Warehouse.Api.Data;
using Warehouse.Api.Models;
using Warehouse.Api.Requests;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase {
    private readonly Database database;

    public ProductsController(Database database) {
        this.database = database;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {
        var products = new List<Product>();

        using var connection = database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT Id, Name, Location, Quantity
            FROM Products;
            """;

        using var command = new MySqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var product = new Product {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Location = reader.GetString(2),
                Quantity = reader.GetInt32(3)
            };

            products.Add(product);
        }

        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductRequests request) {
        using var connection = database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO Products (Name, Location, Quantity)
            VALUES (@Name, @Location, @Quantity);
            """;

        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Name", request.Name); // AddWithValue is used to protect against SQL Injection, by treating the value as text rather than an SQL command.
        command.Parameters.AddWithValue("@Location", request.Location);
        command.Parameters.AddWithValue("@Quantity", request.Quantity);

        await command.ExecuteNonQueryAsync();

        const string idSql = "SELECT LAST_INSERT_ID();";

        using var idCommand = new MySqlCommand(idSql, connection);
        var id = Convert.ToInt32(await idCommand.ExecuteScalarAsync());

        var product = new Product {
            Id = id,
            Name = request.Name,
            Location = request.Location,
            Quantity = request.Quantity
        };

        return StatusCode(StatusCodes.Status201Created, product); // HTTP 201 means that the resource was successfully created.
    }
}