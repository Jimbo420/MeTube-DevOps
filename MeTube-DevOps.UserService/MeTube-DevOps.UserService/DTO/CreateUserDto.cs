using System.ComponentModel.DataAnnotations;

namespace MeTube_DevOps.UserService.DTO
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; } 

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
