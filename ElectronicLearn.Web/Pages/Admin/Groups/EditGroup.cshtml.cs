using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Groups
{
    [PermissionChecker(24)]
    public class EditGroupModel : PageModel
    {
        private readonly ICourseService _courseService;
        public EditGroupModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseGroup CourseGroup { get; set; }

        public void OnGet(int groupId)
        {
            CourseGroup = _courseService.GetGroupById(groupId);
        }

        public IActionResult OnPost()
        {
            ModelState.ClearValidationState("CourseGroup.CourseGroups");
            ModelState.ClearValidationState("CourseGroup.Courses");
            ModelState.ClearValidationState("CourseGroup.SubCourses");
            ModelState.MarkFieldValid("CourseGroup.CourseGroups");
            ModelState.MarkFieldValid("CourseGroup.Courses");
            ModelState.MarkFieldValid("CourseGroup.SubCourses");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _courseService.UpdateGroup(CourseGroup);

            return RedirectToPage("Index");
        }
    }
}
