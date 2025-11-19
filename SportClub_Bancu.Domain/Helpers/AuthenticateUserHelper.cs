using SportClub_Bancu.Domain.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Domain.Helpers
{
    public class AuthenticateUserHelper
    {
        public static ClaimsIdentity Authenticate(User user) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                new Claim("AvatarPath", user.PathImage),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie");

            string email = ClaimTypes.Email; string defaultRoleClaimType = ClaimsIdentity.DefaultRoleClaimType; ;
            //ClaimTypes.Email, ClaimsIdentity.DefaultRoleClaimType);
        }


    }
}
