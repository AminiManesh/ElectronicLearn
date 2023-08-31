using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Course
{
    public class CourseLevel
    {
        public CourseLevel()
        {

        }

        [Key]
        public int CourseLevelId { get; set; }

        [Display(Name = "عنوان سطح")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string CourseLevelTitle { get; set; }

        #region Relations
        public List<Course> Courses { get; set; }
        #endregion
    }
}
