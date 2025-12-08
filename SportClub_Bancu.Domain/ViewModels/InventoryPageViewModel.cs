using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Domain.ViewModels
{
    public class InventoryPageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }
        public int Count { get; set; }

        public decimal Price { get; set; }

        public string? PathImg { get; set; }

    }
    public class PicturesInventoryViewModel
    {
        public string? Path { get; set; }
    }
}
