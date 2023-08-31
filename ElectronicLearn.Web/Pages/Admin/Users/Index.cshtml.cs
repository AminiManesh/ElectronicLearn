using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(2)]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public AdminUsersListViewModel AdminUsersListViewModel { get; set; }
        public void OnGet(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            AdminUsersListViewModel = _userService.GetUsers(pageId, filterEmail, filterUserName);
        }
    }
}
