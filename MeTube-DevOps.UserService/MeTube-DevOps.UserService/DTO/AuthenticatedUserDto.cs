namespace MeTube_DevOps.UserService.DTO
{
    public class AuthenticatedUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}