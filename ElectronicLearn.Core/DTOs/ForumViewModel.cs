using ElectronicLearn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.DTOs
{
    public class QuestionListItemViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public bool IsClosed { get; set; }
        public User User { get; set; }
        public int AnswersCount { get; set; }
        public string CourseName { get; set; }
        public int CourseId { get; set; }
    }
}
