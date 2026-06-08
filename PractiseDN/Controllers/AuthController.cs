using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PractiseDN.Data;
using PractiseDN.Dto;
using PractiseDN.Models;

namespace PractiseDN.Controllers
{
    public class AuthController(AppDbContext _context) : Controller
    {
        public IActionResult Login()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if(existingUser == null)
            {
                var user = new User
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    Username = dto.Username
                };
                _context.Users.Add(user);

                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.ErrorMessage = "User with this email already exists.";
                return View("Register");
            }

                TempData["SuccessMessage"] = "User Created Successfully. Please Login.";
                return RedirectToAction("Login");
        }
    }
}
