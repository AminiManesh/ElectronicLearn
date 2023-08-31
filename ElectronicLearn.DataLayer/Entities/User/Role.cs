using ElectronicLearn.DataLayer.Entities.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.User
{
    public class Role
    {
        public Role()
        {

        }

        [Key]
        public int RoleId { get; set; }

        [Display(Name = "نام نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string RoleTitle { get; set; }

        public bool IsDeleted { get; set; }

        #region Relations
        public List<UserRole>? UserRoles { get; set; }
        public List<RolePermission>? RolePermissions { get; set; }
        #endregion
    }
}
