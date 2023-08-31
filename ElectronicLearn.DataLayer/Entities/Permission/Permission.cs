using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Permission
{
    public class Permission
    {
        public Permission()
        {

        }

        [Key]
        public int PermissionId { get; set; }

        [Display(Name = "عنوان دسترسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string PermissionTitle { get; set; }

        public int? ParentId { get; set; }


        #region Relations
        [ForeignKey("ParentId")]
        public List<Permission> Permissions { get; set; }
        public List<RolePermission>? RolePermissions { get; set; }
        #endregion
    }
}
