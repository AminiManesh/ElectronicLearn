using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Course
{
    public class CourseStatus
    {
        public CourseStatus()
        {

        }

        [Key]
        public int CourseStatusId { get; set; }

        [Display(Name = "عنوان وضعیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string CourseStatusTitle { get; set; }

        #region Relations
        public List<Course> Courses { get; set; }
        #endregion
    }
}