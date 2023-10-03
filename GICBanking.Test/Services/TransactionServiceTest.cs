using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Services;

namespace GICBanking.Test.Services
{
    public class TransactionServiceTest
    {
        private TestDataContextFactory _factory;
        public TransactionServiceTest()
        {
            _factory = new TestDataContextFactory();
        }

        [Fact]
        public async Task GetTransactionListAsync_Postive_WithDataAsync()
        {
            #region Arrange
            using (var context = _factory.Create())
            {
                var tran = new Transaction
                {
                    Id = 40,
                    Amount = 1000,
                    Type = "D",
                    TxnId = "1111",
                    Date = DateTime.Now,
                };

                var acc = new Account
                {
                    Id = 71,
                    Number = "111",
                    Balance = 100,
                    Transactions = new List<Transaction> { tran }
                };

                context.Add(acc);
                context.SaveChanges();
            }
            #endregion


            using (var context = _factory.Create())
            {
                #region Act
                var accountService = new AccountService(context);

                var service = new TransactionService(context, accountService);
                var actual = (await service.GetAllTransactionsAsync()).FirstOrDefault();
                #endregion

                #region Assert
                Assert.Equal(40, actual.Id);
                Assert.Equal(1000, actual.Amount);
                #endregion
            }


        }

        [Fact]
        public async Task AddTransactionAsync_Postive_SaveDateSuccess()
        {
            using (var context = _factory.Create())
            {
                #region Arrange
                var accountService = new AccountService(context);

                var service = new TransactionService(context, accountService);

                var tr = new TransactionDTO()
                {
                    Date = DateTime.Now,
                    Type = "D",
                    Amount = 1000,
                    AccountNumber = "111"
                };

                var acc = new Account()
                {
                    Number = "1111",
                    Balance = 1000
                };
                #endregion

                #region Act
                await service.AddTransactionAsync(tr);
                #endregion


            }

        }

        [Fact]
        public async Task GenerateTransactionsForMonthAsync_Positive_WithDataSuccess()
        {
            using (var context = _factory.Create())
            {
                #region Arrange
                var accountService = new AccountService(context);

                var service = new TransactionService(context, accountService);
                var tran = new Transaction
                {
                    Id = 40,
                    Amount = 1000,
                    Type = "D",
                    TxnId = "1111",
                    Date = new DateTime(2022, 06, 02),
                };

                var acc = new Account
                {
                    Id = 71,
                    Number = "1111",
                    Balance = 100,
                    Transactions = new List<Transaction> { tran }
                };


                context.Add(acc);
                context.SaveChanges();
                #endregion

                #region Act
                var transaction = await service.GenerateTransactionsForMonthAsync("1111", 2022, 06);
                #endregion

                #region Assert

                Assert.True(transaction.Any());
                #endregion 

            }

        }

        [Fact]
        public async Task GenerateTransactionsForMonthAsync_Negative_DataNull()
        {
            using (var context = _factory.Create())
            {
                #region Arrange
                var accountService = new AccountService(context);

                var service = new TransactionService(context, accountService);

                var tr = new TransactionDTO()
                {
                    Date = DateTime.Now,
                    Type = "D",
                    Amount = 1000,
                    AccountNumber = "111"
                };

                var acc = new Account()
                {
                    Number = "1111",
                    Balance = 1000
                };
                #endregion

                #region Act
                var transaction = await service.GenerateTransactionsForMonthAsync("1111", 2022, 06);
                #endregion

                #region Assert

                Assert.False(transaction.Any());
                #endregion 

            }

        }

    }

}