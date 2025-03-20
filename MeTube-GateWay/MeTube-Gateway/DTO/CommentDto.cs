using System.Text.Json.Serialization;

namespace MeTube_GateWay.DTO
{
    public class CommentDto
    {
        public int Id { get; init; }
        public int VideoId { get; init; }
        public int UserId { get; init; }
        public string Content { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
    }
}