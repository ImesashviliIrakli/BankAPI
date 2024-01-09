using System.ComponentModel.DataAnnotations;

namespace BankAPI.Domain.Entities.Bank
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
    }
}
