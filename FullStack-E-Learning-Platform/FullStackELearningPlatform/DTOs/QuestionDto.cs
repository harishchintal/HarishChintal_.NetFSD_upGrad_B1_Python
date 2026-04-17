using System.ComponentModel.DataAnnotations;

namespace FullStackELearningPlatform.DTOs
{
    public class QuestionDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int QuizId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public required string QuestionText { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string OptionA { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string OptionB { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string OptionC { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string OptionD { get; set; }

        [Required]
        [RegularExpression("^[A-D]$")]
        public required string CorrectAnswer { get; set; }
    }
}