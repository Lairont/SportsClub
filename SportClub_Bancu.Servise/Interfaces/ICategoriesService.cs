using SportClub_Bancu.Domain.Response;
using SportClub_Bancu.Domain.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Servise.Interfaces
{
    public interface ICategoriesService
    {
        Task<BaseResponse<CategoriesDb>> CreateCategory(Categories model);
        Task<BaseResponse<CategoriesDb>> UpdateCategory(Guid id, Categories model);
        Task<BaseResponse<bool>> DeleteCategory(Guid id);
        Task<BaseResponse<CategoriesDb>> GetCategoryById(Guid id);
        Task<BaseResponse<IEnumerable<CategoriesDb>>> GetAllCategories();
    }
}
