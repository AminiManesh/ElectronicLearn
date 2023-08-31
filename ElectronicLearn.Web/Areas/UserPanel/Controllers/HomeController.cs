using ElectronicLearn.Core.Convertors;
using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        

        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            return View(_userService.GetUserInformation(userId));
        }



        #region Edit Profile

        [Route("UserPanel/EditProfile", Name = "EditProfile")]
        public IActionResult EditProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());;
            return View(_userService.GetInformationForEditUserProfile(userId));
        }

        [HttpPost]
        [Route("UserPanel/EditProfile", Name = "EditProfile")]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View(profile);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var userEmail = User.FindFirstValue(ClaimTypes.Email).ToString();
            var userName = User.Identity.Name;

            if (profile.Email != userEmail && _userService.IsEmailExists(FixText.FixEmail(profile.Email)))
            {
                ModelState.AddModelError("Email", "این ایمیل قبلا ثبت شده است.");
                return View(profile);
            }

            if (profile.UserName != userName && _userService.IsUserNameExists(profile.UserName))
            {
                ModelState.AddModelError("UserName", "این نام کاربری قبلا ثبت شده است.");
                return View(profile);
            }

            _userService.EditProfile(userId, profile);
            ViewBag.IsSuccess = true;

            // Logout User
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login?EditProfile=true");
        }

        #endregion



        #region Change Password

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (!ModelState.IsValid)
            {
                return View(change);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            if (!_userService.IsOldPasswordCorrect(userId, change.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "رمز عبور فعلی شما نادرست می‌باشد.");
                return View(change);
            }

            ViewBag.IsSuccess = _userService.ChangePassword(userId, change.Password);

            return View();
        }

        #endregion
    }
}
