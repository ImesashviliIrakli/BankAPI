using System.ComponentModel.DataAnnotations;

namespace BankAPI.Domain.Dtos.BankDtos
{
    public class WalletDto
    {
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        [MaxLength(3)]
        public string Currency { get; set; }
    }
}
