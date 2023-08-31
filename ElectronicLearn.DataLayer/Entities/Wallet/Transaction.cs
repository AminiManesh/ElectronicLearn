using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Wallet
{
    public class Transaction
    {
        public Transaction()
        {

        }


        [Key]
        public int TransactionId { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int TypeId { get; set; }

        [Display(Name = "کیف پول")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int WalletId { get; set; }

        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Amount { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string Description { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsPaid { get; set; }

        [Display(Name = "تاریخ و ساعت")]
        public DateTime CreateDate { get; set; }



        #region Relations
        public virtual Wallet Wallet { get; set; }
        [ForeignKey("TypeId")]
        public virtual TransactionType TransactionType { get; set; }
        #endregion
    }
}
