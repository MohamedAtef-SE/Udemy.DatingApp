﻿namespace CQRS.Domain.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public required string URL { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }

        // Navigation Property
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
    }
}