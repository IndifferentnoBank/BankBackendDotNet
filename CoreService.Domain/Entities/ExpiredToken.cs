using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoreService.Domain.Entities;

public class ExpiredToken
{
    [Key]
    public string Key { get; set; }

}