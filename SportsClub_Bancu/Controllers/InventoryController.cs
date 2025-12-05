using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SportClub_Bancu.Domain.Enum;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Response;
using SportClub_Bancu.Domain.ViewModels;
using SportClub_Bancu.Servise;
using SportsClub_Bancu.Domain.ViewModels;
using SportClub_Bancu.Servise.Interfaces;
using SportsClub_Bancu.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using SportClub_Bancu.Domain.Filter;


namespace SportsClub_Bancu.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;

        private readonly ICategoriesService _categoriesService;
        private IMapper _mapper { get; set; }

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public InventoryController(IInventoryService inventoryService, ICategoriesService categoriesService)
        {
            _inventoryService = inventoryService;
            _categoriesService = categoriesService;
            _mapper = mapperConfiguration.CreateMapper();
        }


        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] InventoryFilter filter)
        {
            var result = _inventoryService.GetInventoryByFilter(filter);
            var filteredInventory = _mapper.Map<List<InventoryViewModel>>(result.Data);
            return Json(filteredInventory);
        }



        public async Task<IActionResult> ListOfInventory()
        {

            var response = await _inventoryService.GetAllInventories(Guid.Empty);
            if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.NotFound)
            {

                if (response.Data == null)
                {
                    return View(new List<InventoryViewModel>());
                }
            }

            var result = _categoriesService.GetAllCategories();
            var listOfCategories = _mapper.Map<List<CategoriesViewModel>>(result.Data);

            var listOfInventoryViewModel = _mapper.Map<List<InventoryViewModel>>(response.Data);
            foreach(var item in listOfInventoryViewModel)
            {
                var Pictur = await _inventoryService.GetPictur(item.Id);
                if (Pictur.StatusCode != SportClub_Bancu.Domain.Response.StatusCode.NotFound)
                {
                    item.PathImg = Pictur.Data.Path;
                }

            }
            InventoryListViewModel inventoryListViewModel = new InventoryListViewModel
            {
                AvailableCategories = listOfCategories,
                InventoryItems = listOfInventoryViewModel
            };
                return View(inventoryListViewModel);

        }
    }
}
