using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class DeletedUsersListModel : PageModel
    {
        private readonly IUserService _userService;
        public DeletedUsersListModel(IUserService userService)
        {
            _userService = userService;
        }

        public AdminUsersListViewModel AdminUsersListViewModel { get; set; }
        public void OnGet(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            AdminUsersListViewModel = _userService.GetDeletedUsers(pageId, filterEmail, filterUserName);
        }

        public IActionResult OnPost(int userId)
        {
            _userService.ReturnUser(userId);
            return Redirect("/Admin/Users/DeletedUsersList");
        }
    }
}
