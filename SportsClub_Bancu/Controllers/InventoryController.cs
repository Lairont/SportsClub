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


namespace SportsClub_Bancu.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private IMapper _mapper { get; set; }

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;

            _mapper = mapperConfiguration.CreateMapper();
        }

        public async Task<IActionResult> ListOfInventory()
        {

            var response = await _inventoryService.GetAllInventories();
            if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.NotFound)
            {

                if (response.Data == null)
                {
                    return View(new List<InventoryViewModel>());
                }
            }

            var listOfInventoryViewModel = _mapper.Map<List<InventoryViewModel>>(response.Data);

            return View(listOfInventoryViewModel);
        }
    }
}
