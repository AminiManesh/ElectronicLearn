using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Courses
{
    public class IndexEpisodeModel : PageModel
    {
        private readonly ICourseService _courseService;
        public IndexEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public AdminEpisodeListViewModel AdminEpisodeListViewModel { get; set; }
        public void OnGet(int courseId)
        {
            AdminEpisodeListViewModel = _courseService.GetCourseEpisodes(courseId);
        }
    }
}
