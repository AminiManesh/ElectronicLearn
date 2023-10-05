using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Order
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Count { get; set; }

        #region Relations
        public virtual Order Order { get; set; }
        public virtual Course.Course Course { get; set; }
        #endregion
    }
}
