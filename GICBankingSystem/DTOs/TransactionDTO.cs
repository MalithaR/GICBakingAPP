namespace GICBankingSystem.DTOs
{
    public class TransactionDTO
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
    }
}
