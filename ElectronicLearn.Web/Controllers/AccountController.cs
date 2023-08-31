using Microsoft.AspNetCore.Mvc;
using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.Core.Convertors;
using ElectronicLearn.DataLayer.Entities.User;
using ElectronicLearn.Core.Generators;
using ElectronicLearn.Core.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using ElectronicLearn.Core.Senders;

namespace ElectronicLearn.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;

        public AccountController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }



        #region Register

        [Route("Register", Name = "Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/UserPanel");
            }

            return View();
        }

        [HttpPost]
        [Route("Register", Name = "Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            if (_userService.IsUserNameExists(register.UserName))
            {
                ModelState.AddModelError("UserName", "این نام کاربری قبلا ثبت شده است.");
                return View(register);
            }
            if (_userService.IsEmailExists(FixText.FixEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "این ایمیل قبلا ثبت شده است.");
                return View(register);
            }


            User user = new User()
            {
                UserName = register.UserName,
                Email = FixText.FixEmail(register.Email),
                ActiveCode = TextGenerator.GenerateUniqeCode(),
                IsActive = false,
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                RegisterDate = DateTime.Now,
                Avatar = "avatar.jpg"
            };
            _userService.AddUser(user);

            #region Send Activation Email
            var body = _viewRender.RenderToStringAsync("ActivationEmail", user);
            SendEmail.Send(user.Email, "الکترونیک لرن | فعال سازی حساب کاربری", body);
            #endregion

            return View("RegisterSuccess", user);
        }

        #endregion



        #region Login

        [Route("Login", Name = "Login")]
        public IActionResult Login(bool EditProfile = false)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/UserPanel");
            }

            return View();
        }

        [HttpPost]
        [Route("Login", Name = "Login")]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.LoginUser(login.Email, login.Password);
            if (user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };

                    HttpContext.SignInAsync(principal, properties);
                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی‌باشد.");
                    return View(login);
                }
            }

            ModelState.AddModelError("Email", "اطلاعات ورود صحیح نمی‌باشند.");
            return View(login);
        }

        #endregion



        #region Logout

        [Route("Logout", Name = "Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        #endregion



        #region Active Account

        public IActionResult ActiveAccount(string activeCode)
        {
            ViewBag.IsActive = _userService.ActiveUser(activeCode);
            return View();
        }

        #endregion



        #region Forgot Password

        [Route("ForgotPassword", Name = "ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword", Name = "ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            if (!_userService.IsEmailExists(forgot.Email))
            {
                ModelState.AddModelError("Email", "حساب کاربری با این ایمیل موجود نمی‌باشد.");
                return View(forgot);
            }

            if (!_userService.IsUserActive(_userService.GetUserIdByEmail(forgot.Email)))
            {
                ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی‌باشد");
                return View(forgot);
            }

            #region Send Change Password Email

            var user = _userService.GetUserByEmail(forgot.Email);
            var body = _viewRender.RenderToStringAsync("ResetPasswordEmail", user);
            SendEmail.Send(forgot.Email, "الکترونیک لرن | بازیابی کلمه عبور", body);

            #endregion

            return View("ForgotPasswordSuccess", user);
        }

        #endregion



        #region Reset Password

        public IActionResult ResetPassword(string activeCode)
        {
            if (!_userService.IsActiveCodeExists(activeCode))
            {
                return NotFound();
            }

            var model = new ResetPasswordViewModel()
            {
                ActiveCode = activeCode
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel change)
        {
            if (!ModelState.IsValid)
            {
                return View(change);
            }

            ViewBag.IsSuccess = _userService.ResetPassword(change.ActiveCode, change.Password);

            return View();
        }

        #endregion
    }
}