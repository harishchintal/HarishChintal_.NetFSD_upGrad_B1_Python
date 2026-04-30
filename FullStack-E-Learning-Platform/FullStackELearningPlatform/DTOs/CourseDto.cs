using System.ComponentModel.DataAnnotations;

namespace FullStackELearningPlatform.DTOs
{
    public class CourseDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public required string Description { get; set; }
    }
}