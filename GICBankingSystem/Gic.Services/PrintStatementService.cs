using GICBankingSystem.Data;
using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Core;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Principal;

namespace GICBankingSystem.Gic.Services
{
    public class PrintStatementService : IPrintStatementService
    {
        private readonly DataContext dataContext;
        private readonly IRuleService ruleService;
        private readonly IAccountService accountService;
        private readonly ITransactionService transactionService;

        public PrintStatementService(DataContext dataContext,
            IRuleService ruleService,
            IAccountService accountService,
            ITransactionService transactionService)
        {
            this.dataContext = dataContext;
            this.ruleService = ruleService;
            this.accountService = accountService;
            this.transactionService = transactionService;
        }
          
        public async Task GeneratePrintStatement(PrinStatementDTO prinStatementDTO)
        { 
            var rules = await ruleService.GetAllRulesAsync();
            var transactions = await transactionService.GenerateTransactionsForMonthAsync(prinStatementDTO.Account, prinStatementDTO.Year, prinStatementDTO.Month);
            if(!transactions.Any())
            {
                Console.WriteLine("NO transactions found");
                return;
            }

            var account = await accountService.GetAccountByAccountNumberAsync(prinStatementDTO.Account);
            decimal interestAmount = 0;
            var montEbdDate = DateTime.DaysInMonth(prinStatementDTO.Year, prinStatementDTO.Month);
            for (int i = 1; i <= montEbdDate; i++)
            {
                Decimal eodBalance = 0;
                var transactionDate = new DateTime(prinStatementDTO.Year, prinStatementDTO.Month, i);
                var dayTransaction = transactions.Where(t => t.Date == transactionDate);
                if(!dayTransaction.Any())
                {
                    continue;
                }
                foreach (var item in dayTransaction)
                {
                    if (item.Type == "D")
                    {
                        eodBalance += item.Amount;
                    }
                    else
                    {
                        eodBalance -= item.Amount;
                    }

                }

                var applicableRule = rules.Where(x => x.Date <= transactionDate).LastOrDefault();
                if (applicableRule != null)
                {
                    interestAmount += eodBalance * Convert.ToDecimal(applicableRule.Rate) / 100;

                }
                else
                {
                    Console.WriteLine("NO applicable rule found");
                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Account: " + account.Number);
            Console.WriteLine("| Date | Txn Id | Type | Amount | Balance |");
            foreach (var item in transactions)
            {
                Console.WriteLine("|" + item.Date + "|" + item.TxnId + "|" + item.Type + "|" + item.Amount + "|");
            }
            Console.WriteLine("| Date |    | I | " + Math.Round(interestAmount / 365,4) + " | " + interestAmount + " |");
            Console.WriteLine();

        }
    }
}
