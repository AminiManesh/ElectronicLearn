using ElectronicLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.DTOs
{
    public class AdminUsersListViewModel
    {
        public List<User> Users { get; set; }
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class AdminAddUserViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "فرمت ورودی {0} صحیح نمی‌باشد.")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        public IFormFile? Avatar { get; set; }
    }

    public class AdminEditUserViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "فرمت ورودی {0} صحیح نمی‌باشد.")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string? Password { get; set; }

        [Display(Name = "موجودی")]
        public int Balance { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        public string? ImageName { get; set; }
        public IFormFile? Avatar { get; set; }

        public List<int>? SelectedRoles { get; set; }
    }

    public class AdminDeleteUserViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "فرمت ورودی {0} صحیح نمی‌باشد.")]
        public string Email { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        public string ImageName { get; set; }

        public List<Role> SelectedRoles { get; set; }
    }

    public class AdminCourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseImageName { get; set; }
        public string GroupName { get; set; }
        public int CoursePrice { get; set; }
        public int EpisodeCount { get; set; }
    }

    public class AdminCourseListViewModel
    {
        public List<AdminCourseViewModel> Courses { get; set; }
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }
    }

    public class AdminEpisodeViewModel
    {
        [Required]
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

        [Display(Name = "رایگان")]
        public bool IsFree { get; set; }

        public IFormFile EpisodeFile { get; set; }
    }

    public class AdminEpisodeListViewModel
    {
        public List<AdminEpisodeViewModel> EpisodeList { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
