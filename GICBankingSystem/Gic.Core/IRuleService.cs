using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;

namespace GICBankingSystem.Gic.Core
{
    public interface IRuleService
    {
        Task<Rule?> GetRuleByRuleIDAndDateAsync(string ruleID, DateTime date);
        Task<List<Rule>> GetAllRulesAsync();
        Task AddRuleAsync(RuleDTO ruleDTO);

    }
}
