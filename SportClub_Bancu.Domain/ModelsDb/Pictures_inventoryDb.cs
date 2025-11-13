using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Domain.ModelsDb
{
    [Table("pictures_inventory")]
    public class PicturesInventoryDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("inventoryId")]
        public Guid InventoryId { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("uploadedAt", TypeName = "timestamp")]
        public DateTime UploadedAt { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        public InventoryDb InventoryDb { get; set; }
    }
}
