using System.ComponentModel.DataAnnotations;

namespace BankAPI.Domain.Dtos.BankDtos
{
    public class DepositWithdrawDto
    {
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
