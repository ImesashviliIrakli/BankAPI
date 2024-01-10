using BankAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Domain.Entities.Bank
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public string RemoteAccountNumber { get; set; }
        [MaxLength(3)]
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
    }
}
