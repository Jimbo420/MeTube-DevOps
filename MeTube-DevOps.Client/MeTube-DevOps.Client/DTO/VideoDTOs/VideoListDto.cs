namespace MeTube_DevOps.Client.DTO.VideoDTOs
{
    public class VideoListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UserId { get; set; }
    }
}
