using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElectronicLearn.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICourseService _courseService;
        public AdminController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public JsonResult GetSubGroups(int id)
        {
            var subGroups = new List<SelectListItem>()
            {
                new SelectListItem() {Text="انتخاب کنید...", Value="0"}
            };
            subGroups.AddRange(_courseService.GetAllSubGroupsForManageCourse(id));
            return Json(new SelectList(subGroups, "Value", "Text"));
        }

        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/img/all",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }

            var url = $"/img/all/{fileName}";
            return Json(new { uploaded = true, url });
        }
    }
}
