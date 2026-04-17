namespace FullStackELearningPlatform.DTOs
{
    public class CourseDetailDto
    {
        public int CourseId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<LessonDto>? Lessons { get; set; }
        public List<QuizDto>? Quizzes { get; set; }
    }
}
