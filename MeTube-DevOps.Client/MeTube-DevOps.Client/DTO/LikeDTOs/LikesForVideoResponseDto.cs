namespace MeTube_DevOps.Client.DTO.LikeDTOs
{
    public class LikesForVideoResponseDto
    {
        public int Count { get; set; }
        public IEnumerable<LikeDto> Likes { get; set; }
    }
}
