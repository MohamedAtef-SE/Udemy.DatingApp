﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs.auth
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(20,MinimumLength =8)]
        public string Password { get; set; } = string.Empty;
    }
}
