namespace MeTube_DevOps.Client.DTO.LikeDTOs
{
    public class LikeDto
    {
        public int VideoID { get; set; }
        public int UserID { get; set; }

        public string? VideoTitle { get; set; }
        public string? UserName { get; set; }
    }
}
