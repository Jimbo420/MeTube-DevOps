using System.ComponentModel.DataAnnotations;

namespace MeTube_DevOps.Client.DTO.HistoryDTOs
{
    public class HistoryUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int VideoId { get; set; }

        public DateTime DateWatched { get; set; } = DateTime.UtcNow;
    }
}
