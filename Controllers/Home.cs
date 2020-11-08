using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
 
namespace EmptyApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("validatePassword")]
        public object validatePassword(string password)
        {   
            if (password == null) return StatusCode(400);

            Regex passwordRegex = new Regex(@"(?=.*\d+)(?=.*[A-Z]+)(?=.*[a-z]+)(?=.*[!?\.,_\-\*=]+)");
            bool isMatch = passwordRegex.IsMatch(password);
            bool isValid = password.Length == 16 && isMatch;

            Response status = new Response{ status = isValid };
            return Json(status);
        }

        [Route("ping")]
        public IActionResult ping() => StatusCode(200);
    }
}

public class Response
{
    public bool status { get; set; }
}