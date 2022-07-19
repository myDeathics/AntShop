using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Models.Database;

namespace Shop.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/type/[controller]")]
    public class SubTypeController : ControllerBase
    {
        ApplicationContext db;
        public SubTypeController(ApplicationContext appContext)
        {
            db = appContext;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        async public Task<IResult> Create(subtype subtype)
        {
            await db.subtypes.AddAsync(subtype);
            await db.SaveChangesAsync();

            return Results.Json(subtype);
        }

        [HttpGet]
        async public Task<IResult> Get(int type_Id)
        {
            var types = await db.subtypes.Where(t => t.type_Id == type_Id).ToListAsync();
            return Results.Json(types);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        async public Task<IResult> Delete(int id)
        {
            subtype subtype = await db.subtypes.FindAsync(id);
            db.subtypes.Remove(subtype);
            await db.SaveChangesAsync();

            return Results.Json(subtype);
        }
    }
}
