using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; 
using SportClub_Bancu.Domain.Enum;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using SportClub_Bancu.Domain.Response;
using SportClub_Bancu.Domain.ViewModels;
using SportClub_Bancu.Domain.ViewModels.LoginAndRegistration;
using SportClub_Bancu.Servise;
using SportClub_Bancu.Servise.Interfaces;
using SportsClub_Bancu.Models;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using SporClub_Bancu.DAL;

namespace SportsClub_Bancu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IInventoryService _inventoryService;
        private readonly IBaseStorage<UserDb> _userStorage;
        private IMapper _mapper { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });


        public HomeController(ILogger<HomeController> logger,
                              IAccountService accountService,
                              IWebHostEnvironment appEnvironment,
                              IInventoryService inventoryService,
                              IBaseStorage<UserDb> userStorage)

        {
            _logger = logger;
            _accountService = accountService;
            _appEnvironment = appEnvironment;
            _inventoryService = inventoryService;
            _mapper = mapperConfiguration.CreateMapper();
            _userStorage = userStorage;
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
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Succeeded != true)
                return BadRequest("Аутентификация не удалась.");

            // Берём то, что ты реально добавляешь в Program.cs
            var localPic = result.Principal.FindFirst("local_pic")?.Value;

            User model = new User
            {
                Login = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                Email = result.Principal.FindFirst(ClaimTypes.Email)?.Value,
                PathImage = !string.IsNullOrEmpty(localPic) ? localPic : "/images/user.png"
            };

            var response = await _accountService.IsCreatedAccount(model);

            if (response.StatusCode == SportClub_Bancu.Domain.Response.StatusCode.OK)
            {
                   await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));

                return Redirect(returnUrl);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return BadRequest("Ошибка входа через Google.");
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

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult ServInfo()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Crate()
        {
            var sessionData = HttpContext.Session.GetString("Cart");
            var cart = sessionData == null
                ? new List<InventoryPageViewModel>()
                : JsonConvert.DeserializeObject<List<InventoryPageViewModel>>(sessionData);

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid id)
        {

            var resultInventory = await _inventoryService.GetInvBId(id);
            var resultPictures = await _inventoryService.GetPicturByIdInventory(id);

            if (resultInventory.Data != null)
            {

                var sessionData = HttpContext.Session.GetString("Cart");
                var cart = sessionData == null
                    ? new List<InventoryPageViewModel>()
                    : JsonConvert.DeserializeObject<List<InventoryPageViewModel>>(sessionData);

                cart.Add(new InventoryPageViewModel
                {
                    Id = resultInventory.Data.Id,
                    Name = resultInventory.Data.Name,
                    Price = resultInventory.Data.Price,
                    PathImg = resultPictures.Data?.Path
                });

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }

            return RedirectToAction("Crate");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(Guid id)
        {
            var sessionData = HttpContext.Session.GetString("Cart");
            if (sessionData != null)
            {
                var cart = JsonConvert.DeserializeObject<List<InventoryPageViewModel>>(sessionData);
                var itemToRemove = cart.FirstOrDefault(x => x.Id == id);
                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
                }
            }
            return RedirectToAction("Crate");
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





        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string userName, IFormFile? avatarFile)
        {
            // Ищем email в куках текущего пользователя
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            // Ищем запись в БД
            var userDb = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == email);

            if (userDb == null) return NotFound();

            // Обновляем данные
            userDb.Login = userName;

            if (avatarFile != null && avatarFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                string path = Path.Combine(_appEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }
                userDb.PathImage = "/images/" + fileName;
            }

            // Сохраняем в БД
            await _userStorage.Update(userDb);

            // ВАЖНО: Обновляем Claims, чтобы имя и фото в шапке изменились сразу
            var userModel = _mapper.Map<User>(userDb);
            var identity = AuthenticateUserHelper.Authenticate(userModel);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("Profile");
        }

    }
}