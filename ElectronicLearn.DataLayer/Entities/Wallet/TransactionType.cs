using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Entities.Wallet
{
    public class TransactionType
    {
        public TransactionType()
        {

        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransactionTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }


        #region Relations
        public virtual List<Transaction> Transactions { get; set; }
        #endregion
    }
}
