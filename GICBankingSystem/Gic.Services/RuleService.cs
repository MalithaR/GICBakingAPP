using GICBankingSystem.Data;
using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Core;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Principal;

namespace GICBankingSystem.Gic.Services
{
    public class RuleService : IRuleService
    {
        private readonly DataContext dataContext;

        public RuleService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task AddRuleAsync(RuleDTO ruleDto)
        {
            var rule = MapToRule(ruleDto);
             
            if (!(rule.Rate > 0 && rule.Rate < 100))
            {
                Console.WriteLine("Interest rate should be greater than 0 and less than 100");
                return;
            }

            var existingRule = await GetRuleByRuleIDAndDateAsync(rule.RuleId,rule.Date);
            if (existingRule is null)
            {
                await dataContext.Rules.AddAsync(rule);
            }
            else
            {
                existingRule.Rate = ruleDto.Rate;
                dataContext.Rules.Update(existingRule);
            }

            await dataContext.SaveChangesAsync();
             
            var rules = await GetAllRulesAsync();

            Console.WriteLine();
            Console.WriteLine("Interest rules: ");
            Console.WriteLine("| Date | RuleId | Rate(%) |");
            foreach (var item in rules)
            {
                Console.WriteLine("|" + item.Date + "|" + item.RuleId + "|" + item.Rate + "|");
            }
            Console.WriteLine();

        }

        public async Task<List<Rule>> GetAllRulesAsync()
        {
            return await dataContext.Rules.ToListAsync();
        }

        public async Task<Rule?> GetRuleByRuleIDAndDateAsync(string ruleId,DateTime date)
        {
            return await dataContext.Rules
                .FirstOrDefaultAsync(x => x.RuleId == ruleId && x.Date.Date == date.Date);
        }

        private Rule MapToRule(RuleDTO ruleDto)
        {
            Rule rule = new Rule();
            rule.Date = ruleDto.Date; 
            rule.RuleId = ruleDto.RuleID;
            rule.Rate = ruleDto.Rate; 

            return rule;
        }
    }
}
