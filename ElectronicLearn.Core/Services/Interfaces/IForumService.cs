using ElectronicLearn.Core.DTOs;
using ElectronicLearn.DataLayer.Entities.Course;
using ElectronicLearn.DataLayer.Entities.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services.Interfaces
{
    public interface IForumService
    {
        #region Question
        int AddQuestion(Question question);
        Question GetQuestionById(int questionId);
        Tuple<List<QuestionListItemViewModel>, Course> GetQuestions(int courseId = 0, string search = "");
        int GetUserId(int questionId);
        #endregion

        #region Answer
        void AddAnswer(Answer answer);
        void ChangeAnswerCorrectness(int answerId, bool isCorrect);
        #endregion
    }
}
