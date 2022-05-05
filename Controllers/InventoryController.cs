using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI;
using System.Data;

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

        //An inventory object is created for each row and fills in each column inside the while loop.
        //Each inventory object is added to the inventory list and is then returned to the user as the output.
        //This could be further optimized by creating a function that returns the correct value (either 0 [if null] or reader.GetInt32 || reader.GetString).

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
                Inventory tempInventory;
                while (await reader.ReadAsync())
                {
                    tempInventory = new Inventory();
                    if (reader.IsDBNull(0))
                        tempInventory.InventoryID = 0;
                    else
                        tempInventory.InventoryID = reader.GetInt32(0);

                    if (!reader.IsDBNull(1))
                        tempInventory.APIID = reader.GetString(1);

                    if (reader.IsDBNull(2))
                        tempInventory.SerialNumber = 0;
                    else
                        tempInventory.SerialNumber = reader.GetInt32(2);

                    if (!reader.IsDBNull(3))
                        tempInventory.Name = reader.GetString(3);

                    if (reader.IsDBNull(4))
                        tempInventory.Number = 0;
                    else
                        tempInventory.Number = reader.GetInt32(4);

                    if (reader.IsDBNull(5))
                        tempInventory.StatusID = 0;
                    else
                        tempInventory.StatusID = reader.GetInt32(5);

                    if (reader.IsDBNull(6))
                        tempInventory.OperatorID = 0;
                    else
                        tempInventory.OperatorID = reader.GetInt32(6);

                    if (reader.IsDBNull(7))
                        tempInventory.TractID = 0;
                    else
                        tempInventory.TractID = reader.GetInt32(7);

                    if (reader.IsDBNull(8))
                        tempInventory.InventoryTypeID = 0;
                    else
                        tempInventory.InventoryTypeID = reader.GetInt32(8);

                    inventory.Add(tempInventory);
                }
            }
            return Ok(inventory);
        }

        //This method is meant to create an inventory object and fill in the information based on the id that is given by the user.
        //Currently both retrieve methods use inner join to get foreign key table values but are not being returned but can be added by expanding the code below.

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
                    Inventory tempInventory = new Inventory();
                                        tempInventory = new Inventory();
                    if (reader.IsDBNull(0))
                        tempInventory.InventoryID = 0;
                    else
                        tempInventory.InventoryID = reader.GetInt32(0);

                    if (!reader.IsDBNull(1))
                        tempInventory.APIID = reader.GetString(1);

                    if (reader.IsDBNull(2))
                        tempInventory.SerialNumber = 0;
                    else
                        tempInventory.SerialNumber = reader.GetInt32(2);

                    if (!reader.IsDBNull(3))
                        tempInventory.Name = reader.GetString(3);

                    if (reader.IsDBNull(4))
                        tempInventory.Number = 0;
                    else
                        tempInventory.Number = reader.GetInt32(4);

                    if (reader.IsDBNull(5))
                        tempInventory.StatusID = 0;
                    else
                        tempInventory.StatusID = reader.GetInt32(5);

                    if (reader.IsDBNull(6))
                        tempInventory.OperatorID = 0;
                    else
                        tempInventory.OperatorID = reader.GetInt32(6);

                    if (reader.IsDBNull(7))
                        tempInventory.TractID = 0;
                    else
                        tempInventory.TractID = reader.GetInt32(7);

                    if (reader.IsDBNull(8))
                        tempInventory.InventoryTypeID = 0;
                    else
                        tempInventory.InventoryTypeID = reader.GetInt32(8);
                    return Ok(tempInventory);
                }
            }
            return BadRequest("Inventory not found");
        }


        //This updates the values based on the input, the primary key cannot be updated and currently neither can
        //the 3 foreign keys be updated as they are restricted in the database for updates and can't cascade update.

        [HttpPost]
        public async Task<ActionResult<Inventory>> InsertInventory(Inventory Inventory)
        {
            using (MySqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                using MySqlCommand command = new MySqlCommand("InsertInventory", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //command.Parameters.Add(new MySqlParameter("@InventoryID", Inventory.InventoryID));
                command.Parameters.Add(new MySqlParameter("@APIID", Inventory.APIID));
                command.Parameters.Add(new MySqlParameter("@SerialNumber", Inventory.SerialNumber));
                command.Parameters.Add(new MySqlParameter("@Name", Inventory.Name));
                command.Parameters.Add(new MySqlParameter("@Number", Inventory.Number));
                command.Parameters.Add(new MySqlParameter("@StatusID", Inventory.StatusID));
                //command.Parameters.Add(new MySqlParameter("@OperatorID", Inventory.OperatorID));
                //command.Parameters.Add(new MySqlParameter("@TractID", Inventory.TractID));
                //command.Parameters.Add(new MySqlParameter("@InventoryTypeID", Inventory.InventoryTypeID));

                await command.ExecuteNonQueryAsync();
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
                //command.Parameters.Add(new MySqlParameter("@OperatorID", Inventory.OperatorID));
                //command.Parameters.Add(new MySqlParameter("@TractID", Inventory.TractID));
                //command.Parameters.Add(new MySqlParameter("@InventoryTypeID", Inventory.InventoryTypeID));
                int result = await command.ExecuteNonQueryAsync();
                if (result == 0)
                    return BadRequest("Inventory not found");
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

