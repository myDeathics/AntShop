using Microsoft.AspNetCore.Mvc;
using Shop.Models.Database;
using Shop.Models;
using System.Collections.Generic;
using Shop.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shop.Models.JWT;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Shop.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        ApplicationContext db;
        public UserController(ApplicationContext appContext)
        {
            db = appContext;
        }
        [HttpPost, Route("registration")]
        async public Task<IResult> Registration(user user)
        {
            if (await db.users.FirstOrDefaultAsync(x => x.email == user.email) != null) 
            {
                return Results.BadRequest("user was created");
            }
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
            user.password = hashedPassword;
            var u = await db.users.AddAsync(user);
            await db.SaveChangesAsync();
            basket basket = new basket() { user_Id = u.Entity.Id};
            await db.baskets.AddAsync(basket);
            await db.SaveChangesAsync();
            ClaimsIdentity identity = GetIdentity(user.email, user.password);
            if (identity == null)
            {
                return Results.BadRequest(new { errorText = "Invalid username or password." });
            }
            JwtSecurityToken token = UserService.GenerateJWTToken(identity);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            var response = new
            {
                access_token = encodedJwt,
                email = identity.Name
            };

            return Results.Json(response);
        }

        [HttpPost, Route("login")]
        async public Task<IResult> Login(user user) 
        {
            user? u = await db.users.FirstOrDefaultAsync(x => x.email == user.email);
            if (u == null) return Results.BadRequest("Не существует пользователя с таким email");
            string? hashedPass = u.password;

            if (!BCrypt.Net.BCrypt.Verify(user.password, hashedPass)) return Results.BadRequest("Указан неверный пароль");

            ClaimsIdentity identity = GetIdentity(u.email, u.password);
            JwtSecurityToken token = UserService.GenerateJWTToken(identity);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            var response = new
            {
                access_token = encodedJwt,
                email = identity.Name
            };

            return Results.Json(response);
        }

        [HttpGet, Route("auth")]
        async public Task<IResult> Check (user user) 
        {
            ClaimsIdentity identity = GetIdentity(user.email, user.password);
            JwtSecurityToken token = UserService.GenerateJWTToken(identity);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            var response = new
            {
                access_token = encodedJwt,
                email = identity.Name
            };

            return Results.Json(response);
        }
        private ClaimsIdentity? GetIdentity(string email, string password)
        {
            
            user user = db.users.FirstOrDefault(x => x.email == email && x.password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
