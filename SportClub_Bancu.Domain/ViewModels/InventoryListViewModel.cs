using SportsClub_Bancu.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Domain.ViewModels
{
    public class InventoryListViewModel
    {
        public List<InventoryViewModel> InventoryItems { get; set; }
        public List<CategoriesViewModel> AvailableCategories { get; set; }
    }
}
