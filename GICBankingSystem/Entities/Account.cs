namespace GICBankingSystem.Entities
{
    public class Account : Entity
    {
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction>? Transactions { get; set; }
    }
}
