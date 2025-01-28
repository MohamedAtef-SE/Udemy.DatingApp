﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs.auth
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public string? PhotoURL { get; set; }
        public required string Token { get; set; }

    }
}
