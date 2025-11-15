using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Servise.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<User>> Register(User model);
        Task<BaseResponse<User>> login(User model);
    }
}
