namespace MeTube_DevOps.Client.DTO.HistoryDTOs
{
    public class HistoryDto
    {
        public int VideoId { get; set; }
        public string VideoTitle { get; set; } = string.Empty;
        public DateTime DateWatched { get; set; }
    }
}
