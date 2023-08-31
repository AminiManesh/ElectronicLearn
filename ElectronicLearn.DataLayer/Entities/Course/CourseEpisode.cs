using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Course
{
    public class CourseEpisode
    {
        public CourseEpisode()
        {

        }

        [Key]
        public int CourseEpisodeId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Display(Name = "عنوان قسمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(400, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string CourseEpisodeTitle { get; set; }

        [Display(Name = "زمان قسمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public TimeSpan EpisodeTime { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string EpisodeFileName { get; set; }

        [Display(Name = "رایگان")]
        public bool IsFree { get; set; }

        #region Relations
        public Course Course { get; set; }
        #endregion
    }
}
