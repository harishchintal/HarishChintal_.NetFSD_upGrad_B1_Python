namespace FullStackELearningPlatform.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }

        public required string QuestionText { get; set; }
        public required string OptionA { get; set; }
        public required string OptionB { get; set; }
        public required string OptionC { get; set; }
        public required string OptionD { get; set; }
        public required string CorrectAnswer { get; set; }
    }
}