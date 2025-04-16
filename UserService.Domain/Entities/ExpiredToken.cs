using System.ComponentModel.DataAnnotations;

namespace UserService.Domain.Entities;

public class ExpiredToken
{
    [Key]
    public string Key { get; set; }

}