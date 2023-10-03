using GICBankingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace GICBankingSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Rule> Rules { get; set; }
    }
}
