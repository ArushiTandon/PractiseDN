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
                ViewBag.ErrorMessage = "Invalid product data.";
                return View("ProductForm", new ProductDto());
            }

            if (dto.Id > 0)
            {
                // Edit existing product
                var existing = await context.Products.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (existing == null)
                {
                    ViewBag.ErrorMessage = "Product not found.";
                    return View("ProductForm", dto);
                }

                existing.Name = dto.Name;
                existing.Description = dto.Description;
                existing.Price = dto.Price;
                existing.ProductType = dto.ProductType;

                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Create new product
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

        public async Task<IActionResult> DeleteProduct(int productid)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == productid);

            context.Products.Remove(product);

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> EditProduct(int productid)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == productid);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductType = product.ProductType
            };

            return View("ProductForm", dto);
        }
    }
}
