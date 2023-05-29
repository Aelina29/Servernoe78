using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UniversitiesMVC.Models;

namespace UniversitiesMVC.Controllers
{
    public class AccountController : Controller
    {

        // тестовые данные вместо использования базы данных
        private List<Person> people = new List<Person>
        {
            new Person {Login="admin@gmail.com", Password="12345", Role = "admin" },
            new Person { Login="qwerty@gmail.com", Password="55555", Role = "user" }
        };

   

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return Unauthorized(new { errorText = "Invalid username or password" });
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync(claimsPrincipal).Wait();

            return Ok(new { redirectTo = Url.Action("Index", "Home") });
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            Console.WriteLine("logout");
            HttpContext.SignOutAsync().Wait();
            Console.WriteLine(User.Identity.IsAuthenticated);
            Console.WriteLine(User.Identity.Name);
            return Ok();
        }


        private ClaimsIdentity GetIdentity(string username, string password)
        {
            Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
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
