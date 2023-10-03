using GICBankingSystem.Data;
using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Core;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Principal;
using static GICBankingSystem.Utils.Enum;

namespace GICBankingSystem.Gic.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext dataContext;
        private readonly IAccountService accountService;

        public TransactionService(DataContext dataContext,
            IAccountService accountService)
        {
            this.dataContext = dataContext;
            this.accountService = accountService;
        }
         
        public async Task AddTransactionAsync(TransactionDTO transactionDto)
        { 
            if (!Enum.IsDefined(typeof(TransactionType), transactionDto.Type))
            {
                Console.WriteLine("Invalid transaction type. Please try again");
                return;
            }

            var account = await accountService.GetAccountByAccountNumberAsync(transactionDto.AccountNumber);

            if (account is null)
            { 
                if ((TransactionType)Enum.Parse(typeof(TransactionType), transactionDto.Type) == TransactionType.W)
                {
                    Console.WriteLine("First transaction should be a deposit. Please try again");
                    return;
                }

                account = await accountService.AddAccountAsync(new Account { Number = transactionDto.AccountNumber, Balance = 0 });
            }

            if ((TransactionType)Enum.Parse(typeof(TransactionType), transactionDto.Type) == TransactionType.W && account.Balance < transactionDto.Amount)
            {
                Console.WriteLine("Insufficient balance. Please try again");
                return;
            }

            if ((TransactionType)Enum.Parse(typeof(TransactionType), transactionDto.Type) == TransactionType.W)
            {
                account.Balance -= transactionDto.Amount;
            }
            else
            {
                account.Balance += transactionDto.Amount;
            }

            var transaction = await MapToTransaction(transactionDto, account);
            if (account.Transactions == null)
            {
                account.Transactions = new List<Transaction>();
            }
            account.Transactions.Add(transaction);

            dataContext.Accounts.Update(account);
            //await dataContext.Transactions.AddAsync(transaction);
            await dataContext.SaveChangesAsync();


            var transactions = await GetAllTransactionsAsync();

            Console.WriteLine();
            Console.WriteLine("Account: " + account.Number);
            Console.WriteLine("| Date | Txn Id | Type | Amount |");
            foreach (var item in transactions)
            {
                Console.WriteLine("|" + item.Date + "|" + item.TxnId + "|" + item.Type + "|" + item.Amount + "|");
            }
            Console.WriteLine();
        }
         
        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await dataContext.Transactions.ToListAsync();
        }

        public async Task<List<Transaction>> GenerateTransactionsForMonthAsync(string accountNumber, int year, int month)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var montEbdDate = DateTime.DaysInMonth(year, month);
            var lastDayOfMonth = new DateTime(year, month, montEbdDate); // firstDayOfMonth.AddMonths(1).AddDays(-1);
             
            return await dataContext.Transactions
                .Where(x=>x.Account.Number == accountNumber
                          && x.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth).ToListAsync();
        }

        private async Task<string> GenerateTransactionId(DateTime date, string accountNumber)
        {
            var transactions = (await dataContext.Transactions.Where(x => x.Account.Number == accountNumber)
                .ToListAsync());
            var count = transactions?.Count(x => x.Date == date )?? 0;
            return $"{accountNumber}{date.ToString("yyyyMMdd")}{(count+ 1).ToString().PadLeft(2, '0')}";
                         
        }

        private async Task<Transaction> MapToTransaction(TransactionDTO transactionDto, Account account)
        {
            Transaction transaction = new Transaction();
            transaction.TxnId = await GenerateTransactionId(transactionDto.Date, transactionDto.AccountNumber);
            //transaction.AccountId = account.Id;
            transaction.Amount = transactionDto.Amount;
            transaction.Type = transactionDto.Type;
            transaction.Date = transactionDto.Date;

            return transaction;
        }
         
    }
}
