using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Servise.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<string>> Register(User model);
        Task<BaseResponse<ClaimsIdentity>> Login(User model);

        Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode);

        Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model);

    }
}
