﻿namespace MeTube_DevOps.Client.Models
{
    public class History
    {
        public int VideoId { get; set; }
        public string VideoTitle { get; set; } = string.Empty;
        public DateTime DateWatched { get; set; }

        public Video? Video { get; set; }
    }
}
