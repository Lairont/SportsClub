using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SportClub_Bancu.Domain.Enum;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Response;
using SportClub_Bancu.Domain.ViewModels.LoginAndRegistration;
using SportClub_Bancu.Servise;
using SportClub_Bancu.Servise.Interfaces;
using SportsClub_Bancu.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting; 
using System.IO; 
using System.Net.Http; 

namespace SportsClub_Bancu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService;
        //доб
        private readonly IWebHostEnvironment _appEnvironment; 
        private IMapper _mapper { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });


        public HomeController(ILogger<HomeController> logger, IAccountService accountService, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _accountService = accountService;
            _appEnvironment = appEnvironment; // Инициализация
            _mapper = mapperConfiguration.CreateMapper();
        }

        public IActionResult SiteInformation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmEmail()
        {
            return View();
        }

      [HttpPost]
public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel model)
{
    if (!ModelState.IsValid)
    {
        var user = new User
        {
            Login = model.Login,
            Email = model.Email,
            Password = model.Password,
        };

        var response = await _accountService.ConfirmEmail(user, model.GeneratedCode, model.CodeConfirm);

        if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.OK)
        {
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(response.Data));

            return Ok(new { redirectUrl = Url.Action("SiteInformation", "Home"), message = "Email успешно подтвержден!" });
        }

        return BadRequest(new { message = response.Description ?? "Ошибка подтверждения Email." });
    }

    var errors = ModelState.Values
                           .SelectMany(v => v.Errors)
                           .Select(e => e.ErrorMessage)
                           .ToList();

    var firstError = errors.FirstOrDefault() ?? "Неверные данные для подтверждения.";
    return BadRequest(new { message = firstError });
}


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new User
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var response = await _accountService.Login(user);

                if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                    return Ok(model);
                }

                ModelState.AddModelError("", response.Description);
            }

            var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

            return BadRequest(errors);
        }


        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SiteInformation", "Home");
        }




        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Login = model.Login,
                    Email = model.Email,
                    Password = model.Password,
                };

                var response = await _accountService.Register(user);

                if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.OK)
                {

                    var confirm = new ConfirmEmailViewModel
                    {
                        Login = model.Login,
                        Email = model.Email,
                        Password = model.Password,
                        GeneratedCode = response.Data
                    };

                    return Ok(confirm);
                }
                else
                {

                    return BadRequest(new { message = response.Description ?? "Ошибка регистрации." });
                }
            }

            var errors = ModelState.Values
                                     .SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();


            var firstError = errors.FirstOrDefault() ?? "Неверные данные для регистрации.";
            return BadRequest(new { message = firstError });
        }


        public async Task<IActionResult> GoogleLogin(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse", new { returnUrl }),
                    Parameters = { { "prompt", "select_account" } }
                });
            return new EmptyResult();
        }


        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            try
            {
                // Используем this.HttpContext
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (result?.Succeeded == true)
                {
                    User model = new User
                    {
                        Login = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                        Email = result.Principal.FindFirst(ClaimTypes.Email)?.Value,

                        PathImage = "/" + (await SaveImageInImageUser(result.Principal.FindFirst("picture")?.Value, result) ?? "images/user.png")
                    };

                    var response = await _accountService.IsCreatedAccount(model);

                    if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.OK)
                    {
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(response.Data));
                        return Redirect(returnUrl);
                    }

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }

                return BadRequest("Аутентификация не удалась.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google authentication response processing.");
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        private async Task<string> SaveImageInImageUser(string imageUrl, AuthenticateResult result)
        {
            string filePath = "";

            if (!string.IsNullOrEmpty(imageUrl))
            {
                using (var httpClient = new HttpClient())
                {

                    var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(email)) return "";

                    filePath = Path.Combine("ImageUser", $"{email}-avatar.jpg");

                    try
                    {

                        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);


                        await System.IO.File.WriteAllBytesAsync(Path.Combine(_appEnvironment.WebRootPath, filePath), imageBytes);

                        return filePath;
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogError(ex, "Не удалось скачать изображение пользователя с URL: {ImageUrl}", imageUrl);
                        return ""; 
                    }
                    catch (IOException ex)
                    {
                        _logger.LogError(ex, "Ошибка записи файла изображения на диск: {FilePath}", filePath);
                        return "";
                    }
                }
            }

            return "";
        }


        public IActionResult Contact() // <--- Добавьте этот метод
        {
            return View();
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