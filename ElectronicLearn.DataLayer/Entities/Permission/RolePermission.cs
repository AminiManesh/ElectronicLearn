﻿using ElectronicLearn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Permission
{
    public class RolePermission
    {
        public RolePermission()
        {

        }

        [Key]
        public int RP_Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }


        #region Relations
        public Role Role { get; set; }
        public Permission Permission { get; set; }
        #endregion
    }
}
