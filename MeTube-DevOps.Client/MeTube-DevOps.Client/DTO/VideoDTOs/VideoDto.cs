﻿namespace MeTube_DevOps.Client.DTO.VideoDTOs
{
    public class VideoDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public string VideoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime DateUploaded { get; set; }
        public int UserId { get; set; }

        public bool BlobExists { get; set; }
    }
}
