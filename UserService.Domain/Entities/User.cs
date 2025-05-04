using System.ComponentModel.DataAnnotations;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; } 

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Passport { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [Required]
        public UserRole Role { get; set; }

        //[Required]
        //public string Password { get; set; }

    }
}
