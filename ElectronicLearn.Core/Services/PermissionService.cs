using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Context;
using ElectronicLearn.DataLayer.Entities.Permission;
using ElectronicLearn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ElectronicLearnContext _context;
        public PermissionService(ElectronicLearnContext context)
        {
            _context = context;
        }

        public void AddPermissionsToRole(int roleId, List<int> permissionIds)
        {
            foreach (var permissionId in permissionIds)
            {
                _context.RolePermissions.Add(new RolePermission()
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                });
            }

            _context.SaveChanges();
        }

        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void AddRolesToUser(int userId, List<int> roleIds)
        {
            foreach (var roleId in roleIds)
            {
                _context.UserRole.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }

            _context.SaveChanges();
        }

        public bool CkeckPermission(int userId, int permissionId)
        {
            var userRoles = _context.UserRole
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToList();

            if (!userRoles.Any())
                return false;

            var rolesWithThisPermission = _context.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .Select(rp => rp.RoleId)
                .ToList();

            return userRoles.Any(rolesWithThisPermission.Contains);
        }

        public void DeleteRole(Role role)
        {
            _context.RemoveRange(_context.UserRole.Where(ur => ur.RoleId == role.RoleId));
            _context.RemoveRange(_context.RolePermissions.Where(rp => rp.RoleId == role.RoleId));
            role.IsDeleted = true;
            UpdateRole(role);
        }

        public void DeleteRole(int roleId)
        {
            _context.RemoveRange(_context.UserRole.Where(ur => ur.RoleId == roleId));
            _context.RemoveRange(_context.RolePermissions.Where(rp => rp.RoleId == roleId));
            _context.Roles.Find(roleId).IsDeleted = true;
            _context.SaveChanges();
        }

        public void DeleteUserRoles(int userId)
        {
            _context.UserRole.RemoveRange(_context.UserRole
                .Where(ur => ur.UserId == userId));
        }

        public List<Permission> GetAllPermissions()
        {
            return _context.Permissions.ToList();
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public List<int> GetRolePermissions(int roleId)
        {
            return _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToList();
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public void UpdatePermissionsForRole(int roleId, List<int> permissionIds)
        {
            _context.RemoveRange(_context.RolePermissions.Where(rp => rp.RoleId == roleId));
            AddPermissionsToRole(roleId, permissionIds);
        }

        public void UpdateRole(Role role)
        {
            _context.Update(role);
            _context.SaveChanges();
        }
    }
}