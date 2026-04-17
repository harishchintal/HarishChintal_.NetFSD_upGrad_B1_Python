namespace FullStackELearningPlatform.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public required string Title { get; set; }

        public List<Question>? Questions { get; set; }
    }
}