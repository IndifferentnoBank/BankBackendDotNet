using System.ComponentModel.DataAnnotations;

namespace CoreService.Domain.Entities;

public class ExpiredToken
{
    [Key]
    public string Key { get; set; }

}