using ElectronicLearn.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services.Interfaces
{
    public interface IWalletService
    {
        List<TransactionsViewModel> GetAllTransactions(int userId);
        int AddTransaction(int userId, int amount, string description, bool isDeposit, bool isPaid = false);
        int GetTransactionAmountById(int transactionId);
        void PayTransaction(int transactionId);
        void UpdateUserBalance(int transactionId);
        bool IsUserHasWallet(int userId);
        int GetUserBalance(int userId);
    }
}
