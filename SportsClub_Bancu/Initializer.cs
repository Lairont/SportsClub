
using Microsoft.Extensions.DependencyInjection; 
using SporClub_Bancu.DAL;
using SporClub_Bancu.DAL.Storage;
using SportClub_Bancu.DAL.Storage;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Servise.Interfaces; 
using SportClub_Bancu.Servise.Realizations;

namespace SportClub_Bancu
{
    public static class Initializer
    {
         
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseStorage<UserDb>, UserStorage>();
            services.AddScoped<IBaseStorage<InventoryDb>, InventoryStorage>();   //Добавлено
            services.AddScoped<IBaseStorage<OrdersDb>, OrdersStorage>();           //Добавлено
            services.AddScoped<IBaseStorage<CategoriesDb>, CategoriesStorage>();     //Добавлено
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IInventoryService, InventoryService>();         //Добавлено
            services.AddScoped<IOrdersService, OrdersService>();               //Добавлено
            services.AddScoped<ICategoriesService, CategoriesService>();       //Добавлено

            services.AddControllersWithViews()
                  .AddDataAnnotationsLocalization()
                  .AddViewLocalization();
        }
    }
}

