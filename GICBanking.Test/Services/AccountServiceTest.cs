using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Services;

namespace GICBanking.Test.Services
{
    public class AccountServiceTest
    {
        private TestDataContextFactory _factory;
        public AccountServiceTest()
        {
            _factory = new TestDataContextFactory();
        }

        [Fact]
        public async Task GetAccountByIdAsync_Postive_WithDataAsync()
        {
            #region Arrange
            using (var context = _factory.Create())
            { 
                var acc = new Account
                {
                    Id = 71,
                    Number = "111",
                    Balance = 100
                };

                context.Add(acc);
                context.SaveChanges();
            }
            #endregion

            using (var context = _factory.Create())
            {
                #region Act
                var accountService = new AccountService(context); 
                var actual = (await accountService.GetAccountByIdAsync(71));
                #endregion

                #region Assert
                Assert.Equal(71, actual.Id);
                #endregion
            }

        }

        [Fact]
        public async Task GetAccountByAccountNumberAsync_Postive_WithDataAsync()
        {
            #region Arrange 
            using (var context = _factory.Create())
            {
               
                var acc = new Account
                {
                    Id = 71,
                    Number = "2222",
                    Balance = 100
                };

                context.Add(acc);
                context.SaveChanges();
                

            }
            #endregion

            using (var context = _factory.Create())
            {
                #region Act
                var accountService = new AccountService(context);
                var actual = (await accountService.GetAccountByAccountNumberAsync("2222"));
                #endregion

                #region Assert
                Assert.Equal(71, actual.Id);
                #endregion
            }
             
        }

        [Fact]
        public async Task AddAccountAsync_Positive_SaveDateSuccess()
        {
            using (var context = _factory.Create())
            {
                #region Arrange
                var accountService = new AccountService(context);
                 
                var acc = new Account
                {
                    Id = 71,
                    Number = "1111",
                    Balance = 100
                };
                 
                #endregion

                #region Act
                var transaction = await accountService.AddAccountAsync(acc);
                #endregion

            }

        }


    }

}