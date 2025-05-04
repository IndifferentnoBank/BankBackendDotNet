using System.ComponentModel.DataAnnotations;
using UserService.Domain.Enums;

namespace UserService.Application.Dtos.Requests
{
    public class CreateUserDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Passport { get; set; }

        [Required]
        public List<UserRole> Role { get; set; }

    }
}
