using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporClub_Bancu.DAL; // Оставляем как в твоем IBaseStorage
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportsClub_Bancu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBaseStorage<UserDb> _userStorage;

        public AdminController(IBaseStorage<UserDb> userStorage)
        {
            _userStorage = userStorage;
        }

        // URL будет: /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var users = await _userStorage.GetAll().ToListAsync();

            var model = new AdminDashboardViewModel
            {
                TotalUsers = users.Count,
                TotalAdmins = users.Count(x => x.Role == SportClub_Bancu.Domain.Enum.UserRole.Admin),
                AllUsers = users
            };

            return View(model); // Ищет Views/Admin/Dashboard.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                // Проверка: не удалять самого себя
                var currentUserEmail = User.Identity.Name;
                if (user.Email == currentUserEmail)
                {
                    return Json(new { success = false, message = "Нельзя удалить самого себя!" });
                }

                await _userStorage.Delete(user);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Пользователь не найден" });
        }
    }
}