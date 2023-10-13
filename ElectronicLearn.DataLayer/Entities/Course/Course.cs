using ElectronicLearn.DataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Course
{
    public class Course
    {
        public Course()
        {

        }


        [Key]
        public int CourseId { get; set; }

        [Required]
        public int GroupId { get; set; }

        public int? SubGroupId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int CourseLevelId { get; set; }

        [Required]
        public int CourseStatusId { get; set; }

        [Display(Name = "عنوان دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string CourseTitle { get; set; }

        [Display(Name = "توضیح دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string CourseDescription { get; set; }

        [Display(Name = "قیمت دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int CoursePrice { get; set; }

        [MaxLength(600)]
        public string Tags { get; set; }

        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string? CourseImageName { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? DemoFileName { get; set; }

        public bool IsDeleted { get; set; }

        #region Relations
        [ForeignKey("TeacherId")]
        public virtual User.User? Teacher { get; set; }

        [ForeignKey("GroupId")]
        public virtual CourseGroup? CourseGroup { get; set; }

        [ForeignKey("SubGroupId")]
        public virtual CourseGroup? Group { get; set; }

        public virtual CourseLevel? CourseLevel { get; set; }

        public virtual CourseStatus? CourseStatus { get; set; }
        public virtual List<CourseEpisode>? CourseEpisodes { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
        public virtual List<UserCourse> UsersCourses { get; set; }
        #endregion
    }
}