using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportsClub_Bancu.Models;
using SportClub_Bancu.Domain.ViewModels.LoginAndRegistration;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsClub_Bancu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult SiteInformation()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Ok(model);
            }
               var errors = ModelState.Values.SelectMany(v => v.Errors)
                                           .Select(e =>  e.ErrorMessage)
                                           .ToList();
            return BadRequest(errors);        
        }


        [HttpPost]
        public IActionResult Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Ok(model);
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();

            return BadRequest(errors);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
