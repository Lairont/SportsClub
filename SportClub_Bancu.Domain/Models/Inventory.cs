using SportClub_Bancu.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;


namespace SportClub_Bancu.Domain.ModelsDb
{
    public class Inventory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CategoryId { get; set; }

        public int Count { get; set; }

        public InventoryCondition Condition { get; set; }

        public string InventoryNumber { get; set; }

        public decimal Price { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime? WarrantyUntil { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
