namespace GICBankingSystem.Entities
{
    public class Rule : Entity
    {
        public DateTime Date { get; set; } 
        public string RuleId { get; set; }         
        public float Rate { get; set; }
    }
}
