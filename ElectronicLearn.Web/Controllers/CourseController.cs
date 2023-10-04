using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index(int pageId = 1, string filter = "", string priceType = "all", string orderBy = "createDate", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            ViewBag.pageId = pageId;
            ViewBag.filter = filter;
            ViewBag.orderBy = orderBy;
            ViewBag.priceType = priceType;
            ViewBag.Groups = _courseService.GetAllCourseGroups();
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.startPrice = startPrice;
            ViewBag.endPrice = endPrice;
            return View(_courseService.GetCourses(pageId, filter, priceType, orderBy, startPrice, endPrice, selectedGroups, 9));
        }

        [Route("ShowCourse/{courseId}")]
        public IActionResult ShowCourse(int courseId)
        {
            var course = _courseService.GetCourseDetails(courseId);
            return View(course);
        }
    }
}
