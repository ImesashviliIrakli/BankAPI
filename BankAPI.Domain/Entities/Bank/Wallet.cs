using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace BankAPI.Domain.Entities.Bank
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; } 
        [MaxLength(3)]
        public string Currency { get; set; }
    }
}
