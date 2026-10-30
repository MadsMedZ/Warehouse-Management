using MySql.Data.MySqlClient;

namespace Warehouse.Api.Data;

public class Database {
    private readonly string connectionString;

    public Database(IConfiguration configuration) {
        connectionString = configuration.GetConnectionString("WarehouseDatabase") ?? throw new InvalidOperationException("Database connection string not found");
    }

    public MySqlConnection GetConnection() {
        return new MySqlConnection(connectionString);
    }
}