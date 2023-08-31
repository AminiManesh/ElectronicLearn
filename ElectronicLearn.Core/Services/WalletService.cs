using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Context;
using ElectronicLearn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services
{
    public class WalletService : IWalletService
    {
        private readonly ElectronicLearnContext _context;
        public WalletService(ElectronicLearnContext context)
        {
            _context = context;
        }

        public int AddTransaction(int userId, int amount, string description, bool isPaid = false)
        {
            var user = _context.Users
                .Include(u => u.Wallet)
                .ThenInclude(w => w.Transactions)
                .SingleOrDefault(u => u.UserId == userId);

            var transaction = new Transaction()
            {
                Amount = amount,
                CreateDate = DateTime.Now,
                IsPaid = isPaid,
                Description = description,
                TypeId = 1,
                WalletId = (int)user.WalletId
            };
            user.Wallet.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction.TransactionId;
        }

        public List<TransactionsViewModel> GetAllTransactions(int userId)
        {
            return _context.Transactions
                .Where(t => t.Wallet.UserId == userId && t.IsPaid)
                .Select(t => new TransactionsViewModel()
                {
                    Amount = t.Amount,
                    TransactionType = t.TransactionType.TransactionTypeId,
                    DateTime = t.CreateDate,
                    Description = t.Description
                }).ToList();
        }

        public int GetTransactionAmountById(int transactionId)
        {
            return _context.Transactions.Find(transactionId).Amount;
        }

        public int GetUserBalance(int userId)
        {
            if (_context.Wallets.Any(w => w.UserId == userId))
            {
                return _context.Wallets.SingleOrDefault(w => w.UserId == userId).Cash;
            }

            return -1;
        }

        public bool IsUserHasWallet(int userId)
        {
            return _context.Wallets.Any(u => u.UserId == userId);
        }

        public void PayTransaction(int transactionId)
        {
            _context.Transactions.Find(transactionId).IsPaid = true;
            _context.SaveChanges();
        }

        public void UpdateUserBalance(int transactionId)
        {
            var transaction = _context.Transactions
                .Include(t => t.Wallet)
                .SingleOrDefault(t => t.TransactionId == transactionId);

            transaction.Wallet.Cash += transaction.Amount;
            _context.SaveChanges();
        }
    }
}
