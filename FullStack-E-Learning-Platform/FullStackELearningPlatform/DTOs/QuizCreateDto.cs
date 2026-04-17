using System.ComponentModel.DataAnnotations;

namespace FullStackELearningPlatform.DTOs
{
    public class QuizCreateDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CourseId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string Title { get; set; }
    }
}