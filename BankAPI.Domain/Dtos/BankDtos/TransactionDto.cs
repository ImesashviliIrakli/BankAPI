﻿using System.ComponentModel.DataAnnotations;

namespace BankAPI.Domain.Dtos.BankDtos
{
    public class TransactionDto
    {
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string RemoteAccountNumber { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
