using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace ElectronicLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(10)]
    public class IndexModel : PageModel
    {
        private readonly ICourseService _courseService;
        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public AdminCourseListViewModel AdminCourseListViewModel { get; set; }

        public void OnGet(int pageId = 1, int groupId = 0, int subGroupId = 0, string courseName = "")
        {
            var groups = new List<SelectListItem>();
            AddFirstItem(ref groups);
            groups.AddRange(_courseService.GetAllParentGroupsForManageCourse());
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            var subGroups = new List<SelectListItem>();
            AddFirstItem(ref subGroups);
            subGroups.AddRange(_courseService.GetAllSubGroupsForManageCourse(int.Parse(groups.First().Value)));
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text");

            AdminCourseListViewModel = _courseService.GetAllCoursesForAdmin(pageId, groupId, subGroupId, courseName);
        }

        private static void AddFirstItem(ref List<SelectListItem> list)
        {
            list.Add(new SelectListItem()
            {
                Text = "انتخاب کنید...",
                Value = "0"
            });
        }
    }
}
