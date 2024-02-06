using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.Core.Tools;
using ElectronicLearn.DataLayer.Entities.Course;
using ElectronicLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IOrderService _orderService;
        public CourseController(ICourseService courseService, IOrderService orderService)
        {
            _courseService = courseService;
            _orderService = orderService;
        }

        public IActionResult Index(int pageId = 1, string filter = "", string priceType = "all", string orderBy = "createDate", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            ViewBag.pageId = pageId;
            ViewBag.filter = filter;
            ViewBag.orderBy = orderBy;
            ViewBag.priceType = priceType;
            ViewBag.Groups = _courseService.GetAllCourseGroups();
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.startPrice = startPrice;
            ViewBag.endPrice = endPrice;
            return View(_courseService.GetCourses(pageId, filter, priceType, orderBy, startPrice, endPrice, selectedGroups, 9));
        }

        [Route("ShowCourse/{courseId}")]
        public IActionResult ShowCourse(int courseId, int episodeId = 0)
        {
            var userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier.ToString()));
            }

            var course = _courseService.GetCourseDetails(courseId);
            if (course == null)
            {
                return NotFound();
            }

            if (episodeId != 0)
            {
                if (!course.CourseEpisodes.Any(e => e.CourseEpisodeId == episodeId))
                {
                    return NotFound();
                }

                var episode = course.CourseEpisodes.First(e => e.CourseEpisodeId == episodeId);
                string episodeOnlinePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                    $"wwwroot/Courses/CourseOnline/{courseId}",
                    episode.EpisodeFileName.Replace(".rar", ".mp4"));
                if (!System.IO.File.Exists(episodeOnlinePath))
                {
                    string rarPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Courses/Episodes/{courseId}/{episode.EpisodeFileName}");
                    string videoName = episode.EpisodeFileName.Replace(".rar", ".mp4");
                    string extractPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Courses/CourseOnline/{courseId}");
                    FileTools.ExtractFile(rarPath, videoName, extractPath);
                }

                if (User.Identity.IsAuthenticated)
                {
                    if (!episode.IsFree)
                    {
                        if (!_courseService.IsUserHasCourse(userId, courseId))
                        {
                            return NotFound();
                        }
                    }

                    ViewBag.Episode = episode;
                    ViewBag.episodeOnlinePath = episodeOnlinePath.Split("wwwroot")[1];
                }
                else
                {
                    if (episode.IsFree)
                    {
                        ViewBag.Episode = episode;
                        ViewBag.episodeOnlinePath = episodeOnlinePath.Split("wwwroot")[1];
                    }
                }
            }
            return View(course);
        }

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            int orderId = _orderService.AddOrder(userId, id);
            return Redirect("/UserPanel/Order/ShowOrder/" + orderId);
        }

        [Authorize]
        [Route("DownloadEpisode/{episodeId}")]
        public IActionResult DownloadEpisode(int episodeId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var episode = _courseService.GetEpisodeById(episodeId);
            var episodeFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Courses/Episodes/{episode.CourseId}/{episode.EpisodeFileName}");

            if (episode.IsFree)
            {
                byte[] file = System.IO.File.ReadAllBytes(episodeFilePath);
                return File(file, "application/force-download", episode.EpisodeFileName);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_courseService.IsUserHasCourse(userId, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(episodeFilePath);
                    return File(file, "application/force-download", episode.EpisodeFileName);
                }
            }

            return Forbid();
        }

        [HttpPost]
        public IActionResult PostComment(CourseComment comment)
        {
            comment.IsDeleted = false;
            comment.CreateDate = DateTime.Now;
            comment.IsAdminRed = false;
            comment.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            _courseService.AddComment(comment);

            return View("GetComments", _courseService.GetCourseComments(comment.CourseId));
        }

        public IActionResult GetComments(int id, int pageId = 1)
        {
            return View(_courseService.GetCourseComments(id, pageId));
        }

        public IActionResult CourseVote(int courseId)
        {
            var model = _courseService.GetCourseVotes(courseId);
            return PartialView(model);
        }

        [Authorize]
        public IActionResult SubmitVote(int courseId, bool vote)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            int response = _courseService.SumbitVote(userId, courseId, vote);
            var result = "";
            switch (response)
            {
                case 0:
                    result = "رای شما با موفقیت ثبت شد.";
                    break;
                case 1:
                    result = "رای شما با موفقیت تغییر کرد.";
                    break;
                case 2:
                    result = "شما قبلا همین رای را به این دوره داده‌اید.";
                    break;
                default:
                    break;
            }
            return Json(result);
        }
    }
}
