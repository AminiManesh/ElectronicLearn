using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Courses
{
    public class CreateEpisodeModel : PageModel
    {
        private readonly ICourseService _courseService;
        public CreateEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public AdminEpisodeViewModel AdminEpisodeViewModel { get; set; }

        public void OnGet(int courseId)
        {
            AdminEpisodeViewModel = new AdminEpisodeViewModel();
            AdminEpisodeViewModel.CourseId = courseId;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _courseService.AddEpisode(AdminEpisodeViewModel);

            return Redirect($"/Admin/Courses/IndexEpisode/?courseId={AdminEpisodeViewModel.CourseId}");
        }
    }
}
