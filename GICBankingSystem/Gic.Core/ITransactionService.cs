using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;

namespace GICBankingSystem.Gic.Core
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(TransactionDTO transaction); 
        Task<List<Transaction>> GetAllTransactionsAsync();
        Task<List<Transaction>> GenerateTransactionsForMonthAsync(string accountNumber, int Year, int Month);
         
    }
}
