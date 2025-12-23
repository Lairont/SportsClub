using Microsoft.AspNetCore.Mvc;
using SportClub_Bancu.Domain.ViewModels;
using SportClub_Bancu.Servise.Interfaces;
using Newtonsoft.Json; // Убедись, что этот пакет установлен

namespace SportsClub_Bancu.Controllers
{
    public class CartController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public CartController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // Метод для отображения страницы корзины
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            var response = await _inventoryService.GetInvBId(id);
            var resultPictures = await _inventoryService.GetPicturByIdInventory(id);

            if (response.Data != null)
            {
                var cart = GetCartFromSession();

                // Преобразуем в ViewModel для отображения
                var item = new InventoryPageViewModel
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    Price = response.Data.Price,
                    Notes = response.Data.Notes,
                    PathImg = resultPictures.Data?.Path
                };

                cart.Add(item);
                SaveCartToSession(cart);
            }

            return RedirectToAction("Index");
        }

        // Вспомогательные методы для работы с сессией
        private List<InventoryPageViewModel> GetCartFromSession()
        {
            var sessionData = HttpContext.Session.GetString("Cart");
            return sessionData == null
                ? new List<InventoryPageViewModel>()
                : JsonConvert.DeserializeObject<List<InventoryPageViewModel>>(sessionData);
        }

        private void SaveCartToSession(List<InventoryPageViewModel> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }
    }
}