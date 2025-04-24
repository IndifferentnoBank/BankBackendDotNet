using System.ComponentModel.DataAnnotations;

namespace CreditRatingService.Domain.Entities;

public class ExpiredToken
{
    [Key]
    public string Key { get; set; }

}