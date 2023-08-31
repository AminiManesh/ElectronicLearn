using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElectronicLearn.Web.Pages.Admin.Courses
{
    public class EditCourseModel : PageModel
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        public EditCourseModel(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [BindProperty]
        public Course Course { get; set; }

        public void OnGet(int courseId)
        {
            Course = _courseService.GetCourseById(courseId);

            var parentGroups = _courseService.GetAllParentGroupsForManageCourse();
            ViewData["ParentGroups"] = new SelectList(parentGroups, "Value", "Text", Course.GroupId);

            var subGroups = _courseService.GetAllSubGroupsForManageCourse(int.Parse(parentGroups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text", Course.SubGroupId);

            var teacher = _userService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teacher, "Value", "Text", Course.TeacherId);

            var levels = _courseService.GetAllCourseLevels();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text", Course.CourseLevelId);

            var statuses = _courseService.GetAllCourseStatuses();
            ViewData["Statuses"] = new SelectList(statuses, "Value", "Text", Course.CourseStatusId);
        }

        public IActionResult OnPost(IFormFile? demoVideo, IFormFile imgCourseThumb)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _courseService.UpdateCourse(Course, demoVideo, imgCourseThumb);

            return RedirectToPage("Index");
        }
    }
}
