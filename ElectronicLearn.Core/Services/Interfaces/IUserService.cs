using ElectronicLearn.Core.DTOs;
using ElectronicLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsEmailExists(string email);
        bool IsUserNameExists(string username);
        bool IsActiveCodeExists(string activeCode);
        User GetUserByEmail(string email);
        User GetUserById(int userId, bool? includeWallet = false);
        int GetUserIdByEmail(string email);
        int AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
        void ReturnUser(int userId);
        List<Role> GetUserRoles(int userId);
        List<SelectListItem> GetTeachers();

        User LoginUser(string email, string password);
        bool ActiveUser(string activeCode);
        bool IsUserActive(int userId);

        bool ResetPassword(string activeCode, string password);

        #region User Panel

        UserInformationViewModel GetUserInformation(int userId);
        UserPanelSidebarViewModel GetUserPanelSidebarInformation(int userId);
        EditProfileViewModel GetInformationForEditUserProfile(int userId);
        void EditProfile(int userId, EditProfileViewModel profile);
        bool ChangePassword(int userId, string password);
        bool IsOldPasswordCorrect(int userId, string oldPassword);

        #endregion


        #region Admin Panel
        AdminUsersListViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        AdminUsersListViewModel GetDeletedUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        int AdminAddUser(AdminAddUserViewModel user);
        void AdminUpdateUser(AdminEditUserViewModel user);
        AdminEditUserViewModel GetUserForAdminEdit(int userId);
        AdminDeleteUserViewModel GetUserForAdminDelete(int userId);
        #endregion
    }
}
