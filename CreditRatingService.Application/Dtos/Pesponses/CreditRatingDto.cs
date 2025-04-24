using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingService.Application.Dtos.Pesponses
{
    public class CreditRatingDto
    {
        public Guid UserId { get; set; }
        public int TotalLoans { get; set; }
        public int TotalOverduePayments { get; set; }
        public string Rating { get; set; }
    }
}
