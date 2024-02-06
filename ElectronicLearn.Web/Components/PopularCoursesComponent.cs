using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearn.Web.Components
{
    public class PopularCoursesComponent : ViewComponent
    {
        private readonly ICourseService _courseService;
        public PopularCoursesComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult
                (
                    (IViewComponentResult)
                    View("PopularCourses", _courseService.GetPopularCourses())
                );
        }
    }
}
