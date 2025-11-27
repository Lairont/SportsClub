using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Servise.Interfaces
{
    public interface IOrdersService
    {
        Task<BaseResponse<OrdersDb>> CreateOrder(Orders model);
        Task<BaseResponse<OrdersDb>> UpdateOrder(Guid id, Orders model);
        Task<BaseResponse<bool>> DeleteOrder(Guid id);
        Task<BaseResponse<OrdersDb>> GetOrderById(Guid id);
        Task<BaseResponse<IEnumerable<OrdersDb>>> GetAllOrders();
        Task<BaseResponse<IEnumerable<OrdersDb>>> GetOrdersByUserId(Guid userId);
        Task<BaseResponse<IEnumerable<OrdersDb>>> GetOrdersByInventoryId(Guid inventoryId);

    }
}
