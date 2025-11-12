
using System.ComponentModel.DataAnnotations.Schema;


namespace SportClub_Bancu.Domain.ModelsDb
{
    [Table("categories")]
    public class CategoriesDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }
    }
}
