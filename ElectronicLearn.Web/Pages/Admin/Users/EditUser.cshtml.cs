using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(4)]
    public class EditUserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IWalletService _walletService;
        public EditUserModel(IUserService userService, IPermissionService permissionService, IWalletService walletService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _walletService = walletService;
        }

        [BindProperty]
        public AdminEditUserViewModel AdminEditUserViewModel { get; set; }

        public void OnGet(int userId)
        {
            AdminEditUserViewModel= _userService.GetUserForAdminEdit(userId);

            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> selectedRoles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!_walletService.IsUserHasWallet(AdminEditUserViewModel.UserId))
            {
                AdminEditUserViewModel.Balance = -1;
            }

            _userService.AdminUpdateUser(AdminEditUserViewModel);

            //Edit Roles
            _permissionService.DeleteUserRoles(AdminEditUserViewModel.UserId);
            _permissionService.AddRolesToUser(AdminEditUserViewModel.UserId, selectedRoles);

            return Redirect("/Admin/Users/");
        }
    }
}