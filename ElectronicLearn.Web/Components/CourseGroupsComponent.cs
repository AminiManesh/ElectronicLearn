using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearn.Web.Components
{
    public class CourseGroupsComponent : ViewComponent
    {
        private readonly ICourseService _courseService;
        public CourseGroupsComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult
                (
                    (IViewComponentResult)
                    View("CourseGroups", _courseService.GetAllCourseGroups())
                );
        }
    }
}
