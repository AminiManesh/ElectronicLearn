using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearn.Web.Components
{
    public class LastCoursesComponent : ViewComponent
    {
        private readonly ICourseService _courseService;
        public LastCoursesComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(
                    (IViewComponentResult)
                    View("LastCourses", _courseService.GetCourses().Item1)
                );
        }
    }
}
