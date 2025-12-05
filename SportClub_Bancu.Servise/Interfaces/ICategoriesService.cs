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

        BaseResponse<List<Categories>> GetAllCategories();

    }
}
