using ElectronicLearn.Core.Convertors;
using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Generators;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.Core.Tools;
using ElectronicLearn.DataLayer.Context;
using ElectronicLearn.DataLayer.Entities.User;
using ElectronicLearn.DataLayer.Entities.Wallet;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectronicLearn.Core.Services
{
    public class UserService : IUserService
    {
        private ElectronicLearnContext _context;
        public UserService(ElectronicLearnContext context)
        {
            _context = context;
        }


        public bool IsEmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsUserNameExists(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        // Add User
        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(string email, string password)
        {
            string hashedPassword = PasswordHelper.EncodePasswordMd5(password);
            string fixedEmail = FixText.FixEmail(email);
            return _context.Users.SingleOrDefault(u => u.Email == fixedEmail && u.Password == hashedPassword);
        }

        public bool ActiveUser(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);

            if (user == null || user.IsActive)
            {
                return false;
            }

            user.IsActive = true;
            user.ActiveCode = TextGenerator.GenerateUniqeCode();
            var wallet = new Wallet()
            {
                UserId = user.UserId,
                Cash = 0
            };
            user.Wallet = wallet;

            _context.SaveChanges();
            return true;
        }

        public User GetUserByEmail(string email)
        {
            var fixedEmail = FixText.FixEmail(email);
            return _context.Users.SingleOrDefault(u => u.Email == fixedEmail);
        }
        
        public bool ResetPassword(string activeCode, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);

            if (user == null)
            {
                return false;
            }

            user.ActiveCode = TextGenerator.GenerateUniqeCode();
            user.Password = PasswordHelper.EncodePasswordMd5(password);
            _context.SaveChanges();
            return true;
        }

        public User GetUserById(int userId, bool? includeWallet = false)
        {
            if ((bool)includeWallet)
            {
                return _context.Users
                    .Include(u => u.Wallet)
                    .SingleOrDefault(u => u.UserId == userId);
            }
            return _context.Users.Find(userId);
        }

        public bool IsActiveCodeExists(string activeCode)
        {
            return _context.Users.Any(u => u.ActiveCode == activeCode);
        }

        public UserInformationViewModel GetUserInformation(int userId)
        {
            var user = _context.Users
                .Where(u => u.UserId == userId)
                .Include(u => u.Wallet)
                .Single();
            return new UserInformationViewModel()
            {
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                UserName = user.UserName,
                Wallet = user.Wallet.Cash
            };
        }

        public UserPanelSidebarViewModel GetUserPanelSidebarInformation(int userId)
        {
            var user = _context.Users.Find(userId);
            return new UserPanelSidebarViewModel()
            {
                UserName = user.UserName,
                ImageName = user.Avatar,
                RegisterDate = user.RegisterDate
            };
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public EditProfileViewModel GetInformationForEditUserProfile(int userId)
        {
            var user = _context.Users.Find(userId);
            return new EditProfileViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                AvatarName = user.Avatar
            };
        }

        public void EditProfile(int userId, EditProfileViewModel profile)
        {
            var user = GetUserById(userId);

            if (profile.Avatar != null)
            {
                bool deletePrevious = (profile.AvatarName != "avatar.jpg") ? true : false;
                profile.AvatarName = FileTools.SaveFile(profile.Avatar, "wwwroot/img/UserAvatars", deletePrevious, user.Avatar);
            }

            user.UserName = profile.UserName;
            user.Email = profile.Email;
            user.Avatar = profile.AvatarName;

            UpdateUser(user);
        }

        public bool ChangePassword(int userId, string password)
        {
            var user = GetUserById(userId);

            user.Password = PasswordHelper.EncodePasswordMd5(password);
            _context.SaveChanges();
            return true;
        }

        public bool IsOldPasswordCorrect(int userId, string oldPassword)
        {
            var hasOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u => u.UserId == userId && u.Password == hasOldPassword);
        }

        public AdminUsersListViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> users = _context.Users;

            if (!string.IsNullOrEmpty(filterEmail))
            {
                users = users.Where(u => u.Email.ToLower().Contains(filterEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(filterUserName))
            {
                users = users.Where(u => u.UserName.ToLower().Contains(filterUserName.ToLower()));
            }

            // Show item in each page
            int take = 5;
            int skip = (pageId - 1) * take;

            var result = new AdminUsersListViewModel();

            result.CurrentPage = pageId;
            var count = users.Count();
            if (count % take == 0)
            {
                result.PagesCount = count / take;
            }
            else if (count % take > 0)
            {
                result.PagesCount = (count / take) + 1;
            }
            result.Users = users.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return result;
        }

        public int AdminAddUser(AdminAddUserViewModel user)
        {
            var newUser = new User();
            newUser.UserName = user.UserName;
            newUser.Email = user.Email;
            newUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            newUser.RegisterDate = DateTime.Now;
            var activeCode = TextGenerator.GenerateUniqeCode();
            newUser.ActiveCode = activeCode;

            if (user.Avatar != null)
            {
                newUser.Avatar = FileTools.SaveFile(user.Avatar, "wwwroot/img/UserAvatars");
            }
            else
            {
                newUser.Avatar = "avatar.jpg";
            }

            int userId = AddUser(newUser);

            if (user.IsActive)
            {
                ActiveUser(activeCode);
            }

            return userId;
        }

        public bool IsUserActive(int userId)
        {
            return _context.Users.Find(userId).IsActive;
        }

        public int GetUserIdByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email).UserId;
        }

        public List<Role> GetUserRoles(int userId)
        {
            return _context.UserRole
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role)
                .ToList();
        }

        public void AdminUpdateUser(AdminEditUserViewModel user)
        {
            var updateUser = GetUserById(user.UserId, true);
            updateUser.Email = user.Email;
            updateUser.UserName = user.UserName;

            if (user.Balance != -1)
            {
                updateUser.Wallet.Cash = user.Balance;
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                updateUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            }

            if (user.IsActive)
            {
                ActiveUser(updateUser.ActiveCode);
            }


            if (user.Avatar != null)
            {
                bool deletePrevious = (updateUser.Avatar != "avatar.jpg") ? true : false;
                updateUser.Avatar = FileTools.SaveFile(user.Avatar, "wwwroot/img/UserAvatars", deletePrevious, updateUser.Avatar);
            }

            UpdateUser(updateUser);
        }

        public AdminEditUserViewModel GetUserForAdminEdit(int userId)
        {
            var user = GetUserById(userId, true);

            AdminEditUserViewModel result = new AdminEditUserViewModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                Password = "",
                IsActive = user.IsActive,
                UserId = userId,
                ImageName = user.Avatar,
                SelectedRoles = _context.UserRole.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToList()
            };

            if (user.Wallet == null)
            {
                result.Balance = -1;
            }
            else
            {
                result.Balance = user.Wallet.Cash;
            }

            return result;
        }

        public AdminUsersListViewModel GetDeletedUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> users = _context.Users.IgnoreQueryFilters().Where(u => u.IsDeleted);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                users = users.Where(u => u.Email.ToLower().Contains(filterEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(filterUserName))
            {
                users = users.Where(u => u.UserName.ToLower().Contains(filterUserName.ToLower()));
            }

            // Show item in each page
            int take = 5;
            int skip = (pageId - 1) * take;

            var result = new AdminUsersListViewModel();

            result.CurrentPage = pageId;
            var count = users.Count();
            if (count % take == 0)
            {
                result.PagesCount = count / take;
            }
            else if (count % take > 0)
            {
                result.PagesCount = (count / take) + 1;
            }
            result.Users = users.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return result;
        }

        public void DeleteUser(int userId)
        {
            var user = GetUserById(userId);
            user.IsDeleted = true;
            user.IsActive = false;
            UpdateUser(user);
        }

        public AdminDeleteUserViewModel GetUserForAdminDelete(int userId)
        {
            return _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new AdminDeleteUserViewModel()
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    ImageName = u.Avatar,
                    IsActive = u.IsActive,
                    SelectedRoles = _context.UserRole
                        .Where(ur => ur.UserId == userId)
                        .Include(ur => ur.Role)
                        .Select(ur => ur.Role)
                        .ToList()
                }).Single();
        }

        public void ReturnUser(int userId)
        {
            _context.Users.IgnoreQueryFilters().SingleOrDefault(u => u.UserId == userId).IsDeleted = false;
            _context.SaveChanges();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _context.UserRole
                 .Where(ur => ur.RoleId == 2)
                 .Include(ur => ur.User)
                 .Select(ur => new SelectListItem()
                 {
                     Value = ur.UserId.ToString(),
                     Text = ur.User.UserName
                 }).ToList();
        }
    }
}