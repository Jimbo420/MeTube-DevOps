using System.ComponentModel.DataAnnotations;

namespace MeTube_GateWay.DTO
{
    public class LoginDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; }
    }
}