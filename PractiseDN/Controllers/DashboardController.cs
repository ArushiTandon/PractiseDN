using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PractiseDN.Data;
using PractiseDN.Dto;

namespace PractiseDN.Controllers
{
    public class DashboardController(AppDbContext context) : Controller
    {
        public IActionResult Index()
        {
            var list = context.Products.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ProductType = x.ProductType,
                Price = x.Price
            }).ToList();
            return View(list); //Projection in Linq
        }

        public IActionResult ProductForm() => View();

        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            if (dto == null)
            {
                ViewBag.Error = "Invalid product data.";
                return View("ProductForm");

            }

            context.Products.Add(new Models.Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ProductType = dto.ProductType
            });

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProductAsync (int productid)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == productid);

            context.Products.Remove(product);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
