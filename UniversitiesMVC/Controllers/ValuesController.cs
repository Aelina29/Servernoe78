using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace UniversitiesMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            Console.WriteLine(User.Identity.IsAuthenticated);
            return Ok($"Ваш логин: {User.Identity.Name}");
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
        [Route("getrole")]
        public IActionResult GetRole()
        {
            Console.WriteLine("asdd2");
            return Ok("Ваша роль: администратор");
        }
    }
}
