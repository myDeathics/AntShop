using Microsoft.AspNetCore.Mvc;
using Shop.Models.Database;
using Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{   
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        ApplicationContext db;
        public TypeController(ApplicationContext appContext)
        {
            db = appContext;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        async public Task<IResult> Create(type type)
        {
            await db.types.AddAsync(type);
            await db.SaveChangesAsync();

            return Results.Json(type);
        }

        [HttpGet]
        async public Task<IResult> Get()
        {
            var types = await db.types.ToListAsync();
            return Results.Json(types);
        }

        [Authorize(Roles = "admin")]
        async public Task<IResult> Delete(int id)
        {
            type type = await db.types.FindAsync(id);
            db.types.Remove(type);
            await db.SaveChangesAsync();

            return Results.Json(type);
        }
    }
}
