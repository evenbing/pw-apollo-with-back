using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PW.DataTransferObjects.Transactions
{
    public class CreateTransactionDto
    {
        [Required]
        public string Recipient { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be between {1} and {2}")]
        public int Amount { get; set; }
    }
}
