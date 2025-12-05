using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Domain.Filter
{
    public class InventoryFilter
    {
        public Guid IdInventory { get; set; }
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }


        public List<Guid>? CategoryIds { get; set; }
    }
}
