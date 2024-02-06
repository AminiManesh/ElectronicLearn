using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Question
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Display(Name = "عنوان سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string QuestionTitle { get; set; }

        [Display(Name = "متن سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string QuestionBody { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public DateTime? CloseDate { get; set; }

        [Required]
        public bool IsClosed { get; set; }

        #region Relations
        public User.User User { get; set; }
        public Course.Course Course { get; set; }
        public virtual List<Answer> Answers { get; set; }
        #endregion
    }
}
