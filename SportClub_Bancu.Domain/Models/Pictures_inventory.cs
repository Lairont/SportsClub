using System;
using System.Collections.Generic;
using SportClub_Bancu.Domain.Enum;

namespace SportClub_Bancu.Domain.ModelsDb
{
    public class PicturesInventory
    {
        public Guid Id { get; set; }

        public Guid InventoryId { get; set; }

        public string Path { get; set; }

        public DateTime UploadedAt { get; set; }

        public string? Description { get; set; }

    }
}
