using Microsoft.EntityFrameworkCore;
using InventoryAPI;

namespace InventoryAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Inventory> inventory { get; set; }
    }
}
