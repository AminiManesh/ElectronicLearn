using ElectronicLearn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Order
{
    public class Discount
    {
        [Key]
        public int DiscountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DiscountCode { get; set; }

        [Required]
        public int Percent { get; set; }

        public int? UsableCount { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        #region Relations
        public virtual List<UserDiscount> UsersDiscounts { get; set; }
        #endregion
    }
}
