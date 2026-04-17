using System.ComponentModel.DataAnnotations;

namespace FullStackELearningPlatform.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}