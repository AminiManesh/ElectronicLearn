using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Courses
{
    public class EditEpisodeModel : PageModel
    {
        private readonly ICourseService _courseService;
        public EditEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public AdminEpisodeViewModel AdminEpisodeViewModel { get; set; }

        public void OnGet(int episodeId)
        {
            AdminEpisodeViewModel = _courseService.GetEpisodeForEdit(episodeId);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var test = ModelState.Values.First();
                return Page();
            }

            _courseService.UpdateEpisode(AdminEpisodeViewModel);

            return Redirect($"/Admin/Courses/IndexEpisode/?courseId={AdminEpisodeViewModel.CourseId}");
        }
    }
}
