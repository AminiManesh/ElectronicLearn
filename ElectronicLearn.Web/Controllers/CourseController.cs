using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IOrderService _orderService;
        public CourseController(ICourseService courseService, IOrderService orderService)
        {
            _courseService = courseService;
            _orderService = orderService;
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

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            int orderId = _orderService.AddOrder(userId, id);
            return Redirect("/UserPanel/Order/ShowOrder/" + orderId);
        }
    }
}
