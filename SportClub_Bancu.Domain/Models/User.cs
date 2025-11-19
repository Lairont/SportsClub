using SportClub_Bancu.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Claims;

namespace SportClub_Bancu.Domain.ModelsDb
{
    public class User
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

        public string? PathImage { get; set; }

        public DateTime CreatedAt { get; set; }






        //спросить
    }
}
