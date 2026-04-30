namespace FullStackELearningPlatform.DTOs
{
    public class LessonDto
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int OrderIndex { get; set; }
        public int DurationHours { get; set; }
    }
}
