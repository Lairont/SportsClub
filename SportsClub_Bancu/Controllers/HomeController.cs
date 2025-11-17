using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using SportClub_Bancu.Domain.Enum;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.ViewModels.LoginAndRegistration;
using SportClub_Bancu.Servise;
using SportClub_Bancu.Servise.Interfaces;
using SportsClub_Bancu.Models; 
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks; 

namespace SportsClub_Bancu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService; 
        private readonly IMapper _mapper;

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });
        public HomeController(ILogger<HomeController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
            _mapper = mapperConfiguration.CreateMapper(); 
        }

        public IActionResult SiteInformation()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);


                var response = await _accountService.Login(user);
                if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.OK)
                {
                    return Ok(model);
                }

                ModelState.AddModelError("", response.Description);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            return BadRequest(errors);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);


                var response = await _accountService.Register(user);
                if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.OK)
                {
                    return Ok(model);
                }

                ModelState.AddModelError("", response.Description);
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