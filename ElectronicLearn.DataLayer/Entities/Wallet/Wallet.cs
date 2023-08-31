using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Wallet
{
    public class Wallet
    {
        public Wallet()
        {

        }

        [Key]
        public int WalletId { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int UserId { get; set; }

        [Display(Name = "موجودی")]
        public int Cash { get; set; }


        #region Relations
        public virtual User.User User { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
        #endregion
    }
}
