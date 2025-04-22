using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CreditRatingService.Contracts.Dtos;

namespace CreditRatingService.Application.Dtos.Pesponses
{
    public class overdueCreditTransactionDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required] 
        public bool isOverdue { get; set;}

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }

        [Required]
        public Guid RelatedLoanId { get; set; }
    }
}
