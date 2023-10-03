using FakeItEasy;
using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Core;
using GICBankingSystem.Gic.Services;

namespace GICBanking.Test.Services
{
    public class PrintStatementServiceTest
    {
        private TestDataContextFactory _factory;
        public PrintStatementServiceTest()
        {
            _factory = new TestDataContextFactory();
        }

        [Fact]
        public async Task GeneratePrintStatement_Postive_WithDataAsync()
        { 
            using (var context = _factory.Create())
            {
                #region Arrange
                var ruleService = A.Fake<IRuleService>();
                var accountService = A.Fake<IAccountService>();
                var transactionService = A.Fake<ITransactionService>();

                var statementDTO = new PrinStatementDTO
                {
                    Account = "1111",
                    Year = 2022,
                    Month = 06
                };

                var rule = new Rule
                {
                    Date = new DateTime(2022, 05, 01),
                    Rate = 2.5F,
                    RuleId = "Rule01",
                    Id = 14

                };

                var acc = new Account
                {
                    Id = 71,
                    Number = "1111",
                    Balance = 100
                };

                var tran = new Transaction
                {
                    Id = 40,
                    Amount = 1000,
                    Type = "D",
                    TxnId = "1111",
                    Date = DateTime.Now,
                };

                var printStatement = new PrintStatementService(context, ruleService, accountService, transactionService);
                A.CallTo(() => ruleService.GetAllRulesAsync()).Returns(new List<Rule> { rule });
                A.CallTo(() => accountService.GetAccountByAccountNumberAsync(A<string>.Ignored)).Returns(acc);
                A.CallTo(() => transactionService
                    .GenerateTransactionsForMonthAsync(A<string>.Ignored, A<int>.Ignored, A<int>.Ignored))
                    .Returns(new List<Transaction> { tran });
                #endregion
                #region Act and Assert
                try
                {
                    await printStatement.GeneratePrintStatement(statementDTO);
                }
                catch (Exception ex)
                {

                    throw ex;
                } 
                 
                #endregion
            }

        }
         
         


    }

}