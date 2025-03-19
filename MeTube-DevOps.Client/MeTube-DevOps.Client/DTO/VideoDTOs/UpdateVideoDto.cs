using System.ComponentModel.DataAnnotations;

namespace MeTube_DevOps.Client.DTO.VideoDTOs
{
    public class UpdateVideoDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(30, MinimumLength = 0)]
        public string? Title { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string? Description { get; set; }

        [StringLength(30, MinimumLength = 0)]
        public string? Genre { get; set; }

        public string? VideoUrl { get; set; }

        [Required]
        public int UserId { get; set; }

        //[Required]
        //public int Id { get; set; }

        //[StringLength(30, MinimumLength = 3)]
        //public string? Title { get; set; }

        //[StringLength(255, MinimumLength = 3)]
        //public string? Description { get; set; }

        //[StringLength(30, MinimumLength = 3)]
        //public string? Genre { get; set; }

        //public string? VideoUrl { get; set; }

        //[Required]
        //public int UserId { get; set; }
    }

}
