using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class DeleteUserModel : PageModel
    {
        private readonly IUserService _userService;
        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public AdminDeleteUserViewModel AdminDeleteUserViewModel { get; set; }

        public void OnGet(int userId)
        {
            AdminDeleteUserViewModel = _userService.GetUserForAdminDelete(userId);
        }

        public IActionResult OnPost()
        {
            _userService.DeleteUser(AdminDeleteUserViewModel.UserId);
            return Redirect("/Admin/Users");
        }
    }
}
