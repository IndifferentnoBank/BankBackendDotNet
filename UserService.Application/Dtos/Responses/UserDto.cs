
using System.ComponentModel.DataAnnotations;
using UserService.Domain.Enums;

namespace UserService.Application.Dtos.Responses
{
    public class UserDto
    {
        [Required]
        public Guid Id { get; set; }

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
        public List<UserRole> Role { get; set; }
    }
}
