using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CreditRatingService.Contracts.Dtos
{
    public class LoanTransactionDto
    {
        [Required]
        public Guid Id { get; set; }

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

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionStatus
    {
        Processing,
        Completed,
        Rejected,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        WITHDRAW,
        DEPOSIT,
        AUTOPAY_LOAN,
        PAY_LOAN,
        TAKE_LOAN,
    }
}
