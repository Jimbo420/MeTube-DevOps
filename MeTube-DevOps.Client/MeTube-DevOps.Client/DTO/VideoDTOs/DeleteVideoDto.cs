using System.ComponentModel.DataAnnotations;


namespace MeTube_DevOps.Client.DTO.VideoDTOs
{
    public class DeleteVideoDto
    {
        [Required]
        public int Id { get; set; }
    }
}
