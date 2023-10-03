using GICBankingSystem.Entities;

namespace GICBankingSystem.Gic.Core
{
    public interface IAccountService
    {
        Task<Account> GetAccountByIdAsync(int id);
        Task<Account> GetAccountByAccountNumberAsync(string accountNumber);
        Task<Account> AddAccountAsync(Account account);

    }
}
