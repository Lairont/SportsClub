using SportClub_Bancu.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;


namespace SportClub_Bancu.Domain.ModelsDb
{
    [Table("inventory")]
    public class InventoryDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("categoryId")]
        public Guid CategoryId { get; set; }

        [Column("count")]
        public int Count { get; set; }

        [Column("condition")] 
        public InventoryCondition Condition { get; set; } 

        [Column("inventoryNumber")]
        public string InventoryNumber { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [Column("warrantyUntil", TypeName = "timestamp")]
        public DateTime? WarrantyUntil { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt", TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }

        public CategoriesDb CategoryDb { get; set; }

        public ICollection< OrdersDb> OrdersDb { get; set; }

        public ICollection<PicturesInventoryDb> PicturesInventoryDb { get; set; }
    }
}
