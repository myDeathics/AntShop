using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using RandomDataGenerator;
using Shop.Models.Database;
using Microsoft.EntityFrameworkCore;
using Shop.Services;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        ApplicationContext db;
        IWebHostEnvironment appEnvironment;
        public ProductController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            this.db = context;
            this.appEnvironment = appEnvironment;
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        async public Task<IResult> Create(IFormCollection data, IFormFile image)
        {
            string filename = CreateFilename(image);
            await CopyFile(image, appEnvironment.WebRootPath + "/images/", filename);
            product product = new product()
            {
                name = data["name"],
                price = Convert.ToInt32(data["price"]),
                subtype_Id = Convert.ToInt32(data["subtype_Id"]),
                type_Id = Convert.ToInt32(data["type_Id"]),
                image = filename
            };
            await db.products.AddAsync(product);
            await db.SaveChangesAsync();

            return Results.Json(product);
        }

        [HttpGet]
        async public Task<IResult> Get() 
        {   
            int? type_Id = Convert.ToInt32(HttpContext.Request.Query["type_Id"]);
            int? subtype_Id = Convert.ToInt32(HttpContext.Request.Query["subtype_Id"]);
            int page = Convert.ToInt32(HttpContext.Request.Query["page"]);
            int elementsOnPage = Convert.ToInt32(HttpContext.Request.Query["elementsOnPage"]);

            Pagination pagination = new Pagination(db.products.Count() + 1, elementsOnPage);
            List<product> products = await db.products.ToListAsync();

            if (subtype_Id != null)
            {
                products = await db.products.Where(s => s.subtype_Id == subtype_Id).OrderBy(x => x.name).Skip(pagination.GetElementsWhichGot(page)).Take(pagination.GetElements(page)).ToListAsync();
            }
            if (type_Id != null && subtype_Id == null)
            {
                products = await db.products.Where(x => x.type_Id == type_Id).OrderBy(x => x.name).Skip(pagination.GetElementsWhichGot(page)).Take(pagination.GetElements(page)).ToListAsync();
            }
            return Results.Json(products);
        }

        [HttpGet("{id:int}")]
        async public Task<IResult> GetProductById(int id)
        {
            product product = await db.products.FindAsync(id);
            return Results.Json(product);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        async public Task<IResult> Delete(int id) 
        {
            product product = await db.products.FindAsync(id);
            db.products.Remove(product);
            await db.SaveChangesAsync();

            return Results.Json(product);
        }

        async private Task<IResult> CopyFile(IFormFile image, string path, string filename)
        {
            try
            {
                var fileStream = new FileStream(path + filename, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return Results.Json(image);
        }
        private string CreateFilename(IFormFile file)
        {
            string guid = Guid.NewGuid().ToString();
            return guid + ".jpg";
        }
    }
}
