using System.ComponentModel.DataAnnotations;

namespace FullStackELearningPlatform.DTOs
{
    public class LessonCreateDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CourseId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string Title { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 10)]
        public required string Content { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int OrderIndex { get; set; }

        [Required]
        [Range(0, 24)]
        public int DurationHours { get; set; }
    }
}
