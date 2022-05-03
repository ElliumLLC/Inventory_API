using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("api/Inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InventoryController(IConfiguration config)
        {
            this._configuration = config;
        }

        private MySqlConnection GetConnection()
        {
            string connection_string = this._configuration.GetConnectionString("MySqlConnection");
            return new MySqlConnection(connection_string);
        }

        [HttpGet]
        public async Task<ActionResult<List<Inventory>>> getInventory()
        {
            List<Inventory> inventory = new List<Inventory>();
            using (MySqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                using MySqlCommand command = new MySqlCommand("GetInventory", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    inventory.Add(new Inventory
                    {
                        InventoryID = reader.GetInt32(0),
                        APIID = reader.GetString(1),
                        SerialNumber = reader.GetInt32(2),
                        Name = reader.GetString(3),
                        Number = reader.GetInt32(4),
                        StatusID = reader.GetInt32(5),
                        OperatorID = reader.GetInt32(6),
                        TractID = reader.GetInt32(7),
                        InventoryTypeID = reader.GetInt32(8),
                    });
                }
            }
            return Ok(inventory);
        }

        [HttpGet("GetInventorybyID{id}")]
        public async Task<ActionResult<Inventory>> GetInventorybyID(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                using MySqlCommand command = new MySqlCommand("GetInventorybyID", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@id", id));
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Inventory inventory = new Inventory()
                    {
                        InventoryID = id,
                        APIID = reader.GetString(1),
                        SerialNumber = reader.GetInt32(2),
                        Name = reader.GetString(3),
                        Number = reader.GetInt32(4),
                        StatusID = reader.GetInt32(5),
                        OperatorID = reader.GetInt32(6),
                        TractID = reader.GetInt32(7),
                        InventoryTypeID = reader.GetInt32(8),
                    };
                    return Ok(inventory);
                }
            }
            return BadRequest("Contract not found");
        }

        [HttpPost]
        public async Task<ActionResult<Inventory>> InsertInventory(Inventory Inventory)
        {
            using (MySqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                using MySqlCommand command = new MySqlCommand("InsertInventory", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@APIID", Inventory.APIID));
                command.Parameters.Add(new MySqlParameter("@SerialNumber", Inventory.SerialNumber));
                command.Parameters.Add(new MySqlParameter("@Name", Inventory.Name));
                command.Parameters.Add(new MySqlParameter("@Number", Inventory.Number));
                command.Parameters.Add(new MySqlParameter("@StatusID", Inventory.StatusID));
                command.Parameters.Add(new MySqlParameter("@OperatorID", Inventory.OperatorID));
                command.Parameters.Add(new MySqlParameter("@TractID", Inventory.TractID));
                command.Parameters.Add(new MySqlParameter("@InventoryTypeID", Inventory.InventoryTypeID));
            }
            return Ok(Inventory);
        }

        [HttpPut("UpdateInventory{id}")]
        public async Task<ActionResult<Inventory>> UpdateContracts(Inventory Inventory, int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                using MySqlCommand command = new MySqlCommand("UpdateInventory", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@id", id));
                command.Parameters.Add(new MySqlParameter("@APIID", Inventory.APIID));
                command.Parameters.Add(new MySqlParameter("@SerialNumber", Inventory.SerialNumber));
                command.Parameters.Add(new MySqlParameter("@Name", Inventory.Name));
                command.Parameters.Add(new MySqlParameter("@Number", Inventory.Number));
                command.Parameters.Add(new MySqlParameter("@StatusID", Inventory.StatusID));
                command.Parameters.Add(new MySqlParameter("@OperatorID", Inventory.OperatorID));
                command.Parameters.Add(new MySqlParameter("@TractID", Inventory.TractID));
                command.Parameters.Add(new MySqlParameter("@InventoryTypeID", Inventory.InventoryTypeID));
                int result = await command.ExecuteNonQueryAsync();
                if (result == 0)
                    return BadRequest("Contract not found");
                return Ok(Inventory);
            }
        }
        [HttpDelete("DeleteInventory{id}")]
        public async Task<ActionResult<Inventory>> DeleteInventory(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                using MySqlCommand command = new MySqlCommand("DeleteInventory", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@id", id));
                int s = await command.ExecuteNonQueryAsync();
                if (s == 0)
                    return BadRequest("Inventory not found");
                return Ok(s);
            }
        }
    }
}

