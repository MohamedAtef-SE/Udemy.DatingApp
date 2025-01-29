using System.ComponentModel.DataAnnotations;

namespace API.DTOs.auth
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string KnownAs { get; set; } = null!;

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public string DateOfBirth { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
