using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.Core.Tools;
using ElectronicLearn.DataLayer.Entities.Course;
using ElectronicLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    [PermissionChecker(1014)]
    public class MasterController : Controller
    {
        #region Ctor
        private readonly ICourseService _courseService;
        public MasterController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        #endregion

        [Route("master-courses")]
        public IActionResult MasterCoursesList()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var model = _courseService.GetAllMasterCourses(userId);
            return View(model);
        }

        [Route("course-episodes/{courseId}")]
        public IActionResult EpisodesList(int courseId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            if (!_courseService.IsCourseIdExists(courseId))
            {
                return NotFound();
            }

            if (!_courseService.IsTeacherOfCourse(userId, courseId))
            {
                return RedirectToAction("MasterCoursesList");
            }

            var model = _courseService.GetCourseEpisodes(courseId);

            return View(model);
        }

        [Route("download-episode/{episodeId}")]
        public IActionResult DownloadEpisode(int episodeId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var episode = _courseService.GetEpisodeById(episodeId);
            var episodeFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Courses/Episodes/{episode.CourseId}/{episode.EpisodeFileName}");

            if (User.Identity.IsAuthenticated)
            {
                if (_courseService.IsTeacherOfCourse(userId, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(episodeFilePath);
                    return File(file, "application/force-download", episode.EpisodeFileName);
                }
            }

            return Forbid();
        }

        [HttpGet("insert-episode/{courseId}")]
        public IActionResult InsertEpisode(int courseId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            if (!_courseService.IsCourseIdExists(courseId))
            {
                return NotFound();
            }

            if (!_courseService.IsTeacherOfCourse(userId, courseId))
            {
                return RedirectToAction("MasterCoursesList");
            }

            var model = new InsertEpisodeViewModel
            {
                CourseId = courseId
            };
            return View(model);
        }

        [HttpPost("insert-episode/{courseId}")]
        public IActionResult InsertEpisode(InsertEpisodeViewModel episode)
        {
            if (!ModelState.IsValid)
            {
                return View(episode);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            if (_courseService.IsCourseIdExists(episode.CourseId) && _courseService.IsTeacherOfCourse(userId, episode.CourseId))
            {
                _courseService.MasterAddEpisode(episode);
                return Redirect($"/course-episodes/{episode.CourseId}");
            }

            return BadRequest();
        }

        public IActionResult DropzoneTarget(List<IFormFile> episodeFiles, int courseId)
        {
            if (episodeFiles != null && episodeFiles.Any())
            {
                foreach (var file in episodeFiles)
                {
                    string fileName = $"ElectronicLearn.com - {_courseService.GetCourseNameById(courseId)} - {file.FileName}";
                    string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Courses/Episodes/{courseId}");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fullPath = Path.Combine(path, fileName);

                    FileTools.SaveFileWithCustomName(file, fileName, path, true, fileName);

                    return new JsonResult(new { data = fileName, status = "success" });
                }
            }

            return new JsonResult(new { status = "success" });
        }
    }
}
