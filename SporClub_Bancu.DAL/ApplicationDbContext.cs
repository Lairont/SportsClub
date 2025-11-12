using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace SporClub_Bancu.DAL
{
    public class ApplicationDbContext : DbContext
    {



        public DbSet<UserDb> Users { get; set; }
        public DbSet<InventoryDb> Inventory { get; set; }
        public DbSet<CategoriesDb> Categories { get; set; }
        public DbSet<OrdersDb> Orders { get; set; }
        public DbSet<PicturesInventoryDb> PicturesInventory { get; set; }


        protected readonly IConfiguration configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        } 
    }
}
