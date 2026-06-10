using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PractiseDN.Data;
using PractiseDN.Dto;
using PractiseDN.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            if(dto == null || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                ViewBag.ErrorMessage = "Fields cannot be empty.";
                return View("Register");
            }
            {
                ViewBag.ErrorMessage = "All fields are required.";
                return View("Register");
            }
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

        public async Task<IActionResult> LoginUser(UserDto dto)
        {
            var isUserValid = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);
           
            if (isUserValid == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("Login");

            }
            else
            {
                if (isUserValid.Password == dto.Password) 
                {

                var token = GenerateJwtToken(dto);
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });
                    return RedirectToAction("Index", "Dashboard");

                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid email or password.";
                    return View("Login");
                }
            }
            
        }

        private string GenerateJwtToken(UserDto dto)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("SecretKey");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, dto.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtHandler.CreateToken(tokenDescriptor);

            return jwtHandler.WriteToken(token);
        }
    }
}
