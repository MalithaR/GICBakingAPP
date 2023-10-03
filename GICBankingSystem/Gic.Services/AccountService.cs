using GICBankingSystem.Data;
using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Core;
using Microsoft.EntityFrameworkCore;

namespace GICBankingSystem.Gic.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext dataContext;

        public AccountService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await dataContext.Accounts.FindAsync(id);
        }

        public async Task<Account> GetAccountByAccountNumberAsync(string accountNumber)
        {
            return await dataContext.Accounts.FirstOrDefaultAsync(x => x.Number == accountNumber);
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            await dataContext.Accounts.AddAsync(account);
            await dataContext.SaveChangesAsync();

            return account;
        }
    }
}
