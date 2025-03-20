namespace MeTube_DevOps.Client.DTO.UserDTOs
{
    public class AuthenticatedUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}