using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingService.Contracts.Dtos
{
    public class TariffDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double InterestRate { get; set; }
    }

    public class LoanDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public TariffDto Tariff { get; set; }
        [Required]
        public Guid BankAccountId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public double PaidSum { get; set; }
        [Required]
        public double MonthlyPayment { get; set; }
        [Required]
        public double Debt { get; set; }
    }
}
