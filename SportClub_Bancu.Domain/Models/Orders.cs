using SportClub_Bancu.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportClub_Bancu.Domain.ModelsDb
{
    public class Orders
    {
        public Guid Id { get; set; }

        public Guid InventoryId { get; set; }

        public Guid UserId { get; set; }

        public int Quantity { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime? ReturnedAt { get; set; }

        public DateTime? DueDate { get; set; }

        public string? Notes { get; set; }
    }
}
