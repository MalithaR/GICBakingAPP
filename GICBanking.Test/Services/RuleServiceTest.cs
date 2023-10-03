using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Services;

namespace GICBanking.Test.Services
{
    public class RuleServiceTest
    {
        private TestDataContextFactory _factory;
        public RuleServiceTest()
        {
            _factory = new TestDataContextFactory();
        }

        [Fact]
        public async Task GetRuleByRuleIDAndDateAsync_Postive_WithDataAsync()
        {
            #region Arrange
            using (var context = _factory.Create())
            { 
                var rule = new Rule
                {
                    Id = 35,
                    Date = new DateTime(2022,06,25),
                    RuleId = "Rule01"
                };

                context.Add(rule);
                context.SaveChanges();
            }
            #endregion

            using (var context = _factory.Create())
            {
                #region Act
                var ruleService = new RuleService(context); 
                var actual = (await ruleService.GetRuleByRuleIDAndDateAsync("Rule01", new DateTime(2022, 06, 25)));
                #endregion

                #region Assert
                Assert.Equal(35, actual.Id);
                #endregion
            }

        }

        [Fact]
        public async Task GetAllRulesAsync_Postive_WithDataAsync()
        {
            #region Arrange
            using (var context = _factory.Create())
            {
                var rule = new Rule
                {
                    Id = 35,
                    Date = new DateTime(2022, 06, 25),
                    RuleId = "Rule01"
                };

                context.Add(rule);
                context.SaveChanges();
            }
            #endregion

            using (var context = _factory.Create())
            {
                #region Act
                var ruleService = new RuleService(context);
                var actual = (await ruleService.GetAllRulesAsync());
                #endregion

                #region Assert
                Assert.True(actual.Any());
                #endregion
            }
        }

        [Fact]
        public async Task AddRuleAsync_Positive_SaveDateSuccess()
        {
            using (var context = _factory.Create())
            {
                #region Arrange
                var ruleService = new RuleService(context);
                var rule = new RuleDTO
                {
                    RuleID = "Rule01",
                    Date = new DateTime(2022, 06, 25),
                    Rate =  2.5F
                };
                 
                #endregion

                #region Act
                await ruleService.AddRuleAsync(rule);
                #endregion

            }

        }


    }

}