using GICBankingSystem.Data;
using GICBankingSystem.Gic.Core;
using GICBankingSystem.Gic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GICBankingSystem
{
    public class Startup
    {
        //public static IServiceProvider ConfigureService(IConfiguration configuration)
        //{
        //    var provider = new ServiceCollection()
        //        .AddTransient<ITransactionService,TransactionService>()
        //        .AddTransient<IAccountService, AccountService>()
        //        .AddTransient<IRuleService, RuleService>()
        //        .AddDbContext<DataContext>(options => options.UseSqlite(configuration.GetConnectionString("Sqlite")))
        //        .BuildServiceProvider();

        //    return provider;
        //}
    }
}
