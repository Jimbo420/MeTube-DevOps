using System.ComponentModel.DataAnnotations;

namespace MeTube_DevOps.Client.DTO.HistoryDTOs
{
    public class HistoryCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int VideoId { get; set; }

        public DateTime DateWatched { get; set; }
    }
}
