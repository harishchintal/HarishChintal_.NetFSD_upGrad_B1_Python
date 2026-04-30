using System.ComponentModel.DataAnnotations;

namespace FullStackELearningPlatform.DTOs
{
    public class ResultDto
    {
        [Required]
        [Range(0, 100)]
        public int Score { get; set; }
    }
}