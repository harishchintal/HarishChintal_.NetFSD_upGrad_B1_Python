namespace FullStackELearningPlatform.DTOs
{
    public class QuizDto
    {
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public required string Title { get; set; }
    }
}
