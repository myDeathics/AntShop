using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Models.Database;

namespace Shop.Controllers
{   
    [ApiController]
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        ApplicationContext db;

        public BasketController(ApplicationContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public async Task<IResult> AddProductToBasket(int product_Id, int basket_Id)
        {   
            basket_product product = new basket_product() { product_Id = product_Id, basket_Id = basket_Id};
            await db.basket_products.AddAsync(product);
            await db.SaveChangesAsync();

            return Results.Json(product);
        }
        [HttpDelete]
        public async Task<IResult> DeleteProductFromBasket(int id)
        {   
            basket_product product = new basket_product() { Id = id};
            db.basket_products.Remove(product);
            await db.SaveChangesAsync();

            return Results.Json(product);
        }
        [HttpGet]
        public async Task<IResult> GetAllProductsFromBasket(int basketId)
        {
            var products = await db.basket_products.Where(e => e.basket_Id == basketId).ToListAsync();
            return Results.Json(products);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductFromBasketById(int product_Id)
        {
            return RedirectToAction("GetProductById", "ProductController", product_Id);
        }
    }
}
