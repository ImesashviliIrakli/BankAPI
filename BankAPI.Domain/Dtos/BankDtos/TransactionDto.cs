using BankAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Domain.Dtos.BankDtos
{
    public class TransactionDto
    {
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public string RemoteAccountNumber { get; set; }
        [MaxLength(3)]
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
    }
}
