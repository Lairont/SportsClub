using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportClub_Bancu.Domain.ModelsDb;


namespace SporClub_Bancu.DAL
{
    public class ApplicationDbContext : DbContext
    {



        public DbSet<UserDb> UsersDb { get; set; }
        public DbSet<InventoryDb> InventoryDb { get; set; }
        public DbSet<CategoriesDb> CategoriesDb { get; set; }
        public DbSet<OrdersDb> OrdersDb { get; set; }
        public DbSet<PicturesInventoryDb> PicturesInventoryDb { get; set; }


        protected readonly IConfiguration configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        } 
    }
}
