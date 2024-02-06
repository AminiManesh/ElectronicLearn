using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Context;
using ElectronicLearn.DataLayer.Entities.Course;
using ElectronicLearn.DataLayer.Entities.Question;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services
{
    public class ForumService : IForumService
    {
        private readonly ElectronicLearnContext _context;
        public ForumService(ElectronicLearnContext context)
        {
            _context = context;
        }

        public void AddAnswer(Answer answer)
        {
            _context.Answers.Add(answer);
            _context.SaveChanges();
        }

        public int AddQuestion(Question question)
        {
            question.CreateDate = DateTime.Now;
            question.IsClosed = false;
            question.ModifiedDate = DateTime.Now;

            _context.Questions.Add(question);
            _context.SaveChanges();
            return question.QuestionId;
        }

        public void ChangeAnswerCorrectness(int answerId, bool isCorrect)
        {
            _context.Answers.Find(answerId).IsCorrect = isCorrect;
            _context.SaveChanges();
        }

        public int GetUserId(int questionId)
        {
            return _context.Questions.Find(questionId).UserId;
        }

        public Question GetQuestionById(int questionId)
        {
            return _context.Questions
                .Include(x => x.User)
                .Include(q => q.Answers)
                .ThenInclude(a => a.User)
                .First(q => q.QuestionId == questionId);
        }

        public Tuple<List<QuestionListItemViewModel>, Course> GetQuestions(int courseId = 0, string search = "")
        {
            IQueryable<QuestionListItemViewModel> item1 = _context.Questions
                .Where(q => q.QuestionTitle.Contains(search))
                .Include(q => q.User)
                .Include(q => q.Answers)
                .Include(q => q.Course)
                .Select(s => new QuestionListItemViewModel
                {
                    QuestionId = s.QuestionId,
                    QuestionTitle = s.QuestionTitle,
                    CreateDate = s.CreateDate,
                    ModifiedDate = s.ModifiedDate,
                    CloseDate = s.CloseDate,
                    IsClosed = s.IsClosed,
                    AnswersCount = s.Answers.Count(),
                    User = s.User,
                    CourseName = s.Course.CourseTitle,
                    CourseId = s.CourseId
                })
                .OrderByDescending(q => q.CreateDate);

            Course item2 = new Course();
            if (courseId != 0)
            {
                item1 = item1.Where(q => q.CourseId == courseId);
                item2 = _context.Courses.Find(courseId);
            }
            
            return Tuple.Create(item1.ToList(), item2);
        }
    }
}
