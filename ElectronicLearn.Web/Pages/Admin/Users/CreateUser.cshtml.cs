using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(3)]
    public class CreateUserModel : PageModel
    {
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;
        public CreateUserModel(IPermissionService permissionService, IUserService userService)
        {
            _permissionService = permissionService;
            _userService = userService;
        }

        [BindProperty]
        public AdminAddUserViewModel? AdminAddUserViewModel { get; set; }

        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = _userService.AdminAddUser(AdminAddUserViewModel);

            // Add Roles
            _permissionService.AddRolesToUser(userId, SelectedRoles);


            return Redirect("/Admin/Users");
        }
    }
}
