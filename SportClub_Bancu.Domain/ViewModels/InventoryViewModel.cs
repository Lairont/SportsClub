 namespace SportsClub_Bancu.Domain.ViewModels
{
    public class InventoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }
        public int Count { get; set; }

        public decimal Price { get; set; }

        public string? PathImg { get; set; }
    }
}
