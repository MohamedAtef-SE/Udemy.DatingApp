﻿namespace CQRS.Domain.Entities
{
    public class UserLike
    {
        // SourceUser: the user who doing the like
        // TargetUser: the user who is been liked
        public AppUser SourceUser { get; set; } = null!;
        public int SourceUserId { get; set; }
        public AppUser TargetUser { get; set; } = null!;
        public int TargetUserId { get; set; }
    }
}
