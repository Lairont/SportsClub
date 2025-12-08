using SportClub_Bancu.Domain.Filter;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Servise.Interfaces
{
    public interface IInventoryService
    {
        // Inventory 
        Task<BaseResponse<InventoryDb>> CreateInventory(Inventory model);
        Task<BaseResponse<InventoryDb>> UpdateInventory(Guid id, Inventory model);
        Task<BaseResponse<bool>> DeleteInventory(Guid id);
        Task<BaseResponse<InventoryDb>> GetInventoryById(Guid id);
        Task<BaseResponse<List<Inventory>>> GetAllInventories();
        Task<BaseResponse<List<Inventory>>> GetAllInventoriesById(Guid IdInventory);

        BaseResponse<List<Inventory>> GetInventoryByFilter(InventoryFilter filter);
        Task<BaseResponse<PicturesInventory>> GetPictur(Guid id);


        Task<BaseResponse<Inventory>> GetInvBId(Guid id);
        Task<BaseResponse<PicturesInventory>> GetPicturByIdInventory(Guid id);

    }
}
