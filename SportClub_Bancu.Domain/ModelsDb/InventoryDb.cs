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

        [Column("condition")] // обычно строка или enum, но в БД — скорее int/varchar
        public InventoryCondition Condition { get; set; } // или InventoryCondition Condition, если используете enum

        [Column("inventoryNumber")]
        public string InventoryNumber { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [Column("warrantyUntil")]
        public DateTime? WarrantyUntil { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt", TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }
    }
}
