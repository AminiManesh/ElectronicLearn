using ElectronicLearn.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Courses
{
    public class CreateEpisodeModel : PageModel
    {
        public AdminEpisodeViewModel AdminEpisodeViewModel { get; set; }
        public void OnGet(int courseId)
        {
            AdminEpisodeViewModel = new AdminEpisodeViewModel();
            AdminEpisodeViewModel.CourseId = courseId;
        }

        public void OnPost()
        {

        }
    }
}
