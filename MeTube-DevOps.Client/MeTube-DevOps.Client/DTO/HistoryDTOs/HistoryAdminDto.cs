namespace MeTube_DevOps.Client.DTO.HistoryDTOs
{
    public class HistoryAdminDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VideoId { get; set; }
        public string? UserName { get; set; }
        public string? VideoTitle { get; set; }
        public DateTime DateWatched { get; set; }
    }
}
