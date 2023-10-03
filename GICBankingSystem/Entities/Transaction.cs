namespace GICBankingSystem.Entities
{
    public class Transaction : Entity
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string TxnId { get; set; }
         
        public Account Account { get; set; }
    }
}
