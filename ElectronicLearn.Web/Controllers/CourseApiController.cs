using ElectronicLearn.DataLayer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearn.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseApiController : ControllerBase
    {
        private readonly ElectronicLearnContext _context;
        public CourseApiController(ElectronicLearnContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var courseTitles = _context.Courses
                    .Where(c => c.CourseTitle.Contains(term))
                    .Select(c => c.CourseTitle)
                    .ToList();
                return Ok(courseTitles); 
            }
            catch 
            {
                return BadRequest();
            }
        }
    }
}
