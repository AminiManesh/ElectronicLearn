using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Question;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicLearn.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumService _forumService;
        private readonly ICourseService _courseService;
        public ForumController(IForumService forumService, ICourseService courseService)
        {
            _forumService = forumService;
            _courseService = courseService;
        }

        public IActionResult Index(string search = "")
        {
            return View(_forumService.GetQuestions(search: search));
        }

        #region Create Question
        [Authorize]
        public IActionResult CreateQuestion(int id)
        {
            Question question = new Question
            {
                CourseId = id,
            };

            return View(question);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateQuestion(Question question)
        {
            ModelState.ClearValidationState("User");
            ModelState.ClearValidationState("Course");
            ModelState.ClearValidationState("Answers");
            ModelState.MarkFieldSkipped("User");
            ModelState.MarkFieldSkipped("Course");
            ModelState.MarkFieldSkipped("Answers");

            question.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier.ToString()));
            var sanitizer = new HtmlSanitizer();
            question.QuestionBody = sanitizer.Sanitize(question.QuestionBody);
            if (!ModelState.IsValid)
            {
                return View(question);
            }

            int questionId = _forumService.AddQuestion(question);
            return Redirect($"/Forum/ShowQuestion/{questionId}");
        }
        #endregion

        #region Show Question
        public IActionResult ShowQuestion(int id)
        {
            var model = _forumService.GetQuestionById(id);
            return View(model);
        }
        #endregion

        #region Course Questions
        public IActionResult CourseQuestions(int id, string search = "")
        {
            return View(_forumService.GetQuestions(id, search));
        }
        #endregion

        #region Post Answer
        [HttpPost]
        [Authorize]
        public IActionResult PostAnswer(Answer answer)
        {
            if (!string.IsNullOrEmpty(answer.AnswerBody))
            {
                answer.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier.ToString()));
                answer.CreateDate = DateTime.Now;
                var sanitizer = new HtmlSanitizer();
                answer.AnswerBody = sanitizer.Sanitize(answer.AnswerBody);
                _forumService.AddAnswer(answer);
            }

            return Redirect($"/Forum/ShowQuestion/{answer.QuestionId}");
        }
        #endregion

        #region Confirm Answer
        public IActionResult ConfirmAnswer(int id, int answerId, bool isCorrect)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier.ToString()));
                if (_forumService.GetUserId(id) == userId)
                {
                    _forumService.ChangeAnswerCorrectness(answerId, isCorrect);
                }
            }

            return RedirectToAction("ShowQuestion", new { id });
        }
        #endregion
    }
}
