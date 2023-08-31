using ElectronicLearn.DataLayer.Entities.Permission;
using ElectronicLearn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Roles
        List<Role> GetRoles();
        Role GetRoleById(int roleId);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        void DeleteRole(int roleId);
        void AddRolesToUser(int userId, List<int> roleIds);
        void DeleteUserRoles(int userId);
        #endregion


        #region Permissions
        List<Permission> GetAllPermissions();
        void AddPermissionsToRole(int roleId, List<int> permissionIds);
        void UpdatePermissionsForRole(int roleId, List<int> permissionIds);
        List<int> GetRolePermissions(int roleId);
        bool CkeckPermission(int userId, int permissionId);
        #endregion
    }
}
