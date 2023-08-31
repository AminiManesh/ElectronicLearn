using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class EditRoleModel : PageModel
    {
        private readonly IPermissionService _permissionService;
        public EditRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role Role{ get; set; }

        public void OnGet(int roleId)
        {
            Role = _permissionService.GetRoleById(roleId);
            ViewData["Permissions"] = _permissionService.GetAllPermissions();
            ViewData["SelectedPermissions"] = _permissionService.GetRolePermissions(roleId);
        }

        public IActionResult OnPost(List<int> selectedPermissions)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _permissionService.UpdateRole(Role);
            _permissionService.UpdatePermissionsForRole(Role.RoleId, selectedPermissions);

            return RedirectToPage("Index");
        }
    }
}
