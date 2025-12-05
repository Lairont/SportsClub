using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportClub_Bancu.Domain.ViewModels;
using SportClub_Bancu.Servise;
using SportClub_Bancu.Servise.Interfaces;

namespace SportsClub_Bancu.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private IMapper _mapper { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
            _mapper = mapperConfiguration.CreateMapper();
        }

        public IActionResult ListOfCategories()
        {
            var result = _categoriesService.GetAllCategories();
            var listOfCategories = _mapper.Map<List<CategoriesViewModel>>(result.Data);
            return View(listOfCategories);
        }
    }
}
