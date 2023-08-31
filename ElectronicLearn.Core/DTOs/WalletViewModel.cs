using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.DTOs
{
    public class ChargeWalletViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Amount { get; set; }
    }

    public class TransactionsViewModel
    {
        public int Amount { get; set; }
        public int TransactionType { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
    }
}
