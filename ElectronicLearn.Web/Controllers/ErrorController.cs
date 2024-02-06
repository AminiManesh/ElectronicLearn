using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearn.Web.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        [HttpGet("{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            Response.StatusCode = statusCode;
            return View(statusCode);
        }
    }
}
