using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElectronicLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(11)]
    public class CreateCourseModel : PageModel
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        public CreateCourseModel(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [BindProperty]
        public Course Course { get; set; }

        public void OnGet()
        {
            var parentGroups = _courseService.GetAllParentGroupsForManageCourse();
            ViewData["ParentGroups"] = new SelectList(parentGroups, "Value", "Text");

            var subGroups = _courseService.GetAllSubGroupsForManageCourse(int.Parse(parentGroups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text");

            var teacher = _userService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teacher, "Value", "Text");

            var levels = _courseService.GetAllCourseLevels();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text");

            var statuses = _courseService.GetAllCourseStatuses();
            ViewData["Statuses"] = new SelectList(statuses, "Value", "Text");
        }

        public IActionResult OnPost(IFormFile demoVideo, IFormFile imgCourseThumb)
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.AddCourse(Course, demoVideo, imgCourseThumb);

            return RedirectToPage("Index");
        }
    }
}