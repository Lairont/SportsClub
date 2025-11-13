using SportClub_Bancu.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportClub_Bancu.Domain.ModelsDb
{
    [Table("orders")]
    public class OrdersDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("inventoryId")]
        public Guid InventoryId { get; set; }

        [Column("userId")]
        public Guid UserId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("status")] 
        public OrderStatus Status { get; set; } 

        [Column("issuedAt", TypeName = "timestamp")]
        public DateTime IssuedAt { get; set; }

        [Column("returnedAt", TypeName = "timestamp")]
        public DateTime? ReturnedAt { get; set; }

        [Column("dueDate", TypeName = "timestamp")]
        public DateTime? DueDate { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        public ICollection<InventoryDb> InventoryDb { get; set; }

        public UserDb UserDb { get; set; }
    }
}
