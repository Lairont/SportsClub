using SportClub_Bancu.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;


namespace SportClub_Bancu.Domain.ModelsDb
{
    public class Categories
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
