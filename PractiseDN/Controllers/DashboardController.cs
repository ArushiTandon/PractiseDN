using Microsoft.AspNetCore.Mvc;
using PractiseDN.Data;
using PractiseDN.Dto;

namespace PractiseDN.Controllers
{
    public class DashboardController(AppDbContext context) : Controller
    {
        public IActionResult Index()
        {
            var list = context.Products.Select(x => new ProductDto { Id = x.Id, Name = x.Name, 
             Description = x.Description, ProductType = x.ProductType, Price = x.Price }).ToList();
            return View(list); //Projection in Linq
        }

        public IActionResult ProductForm() => View();

    }
}
