namespace FullStackELearningPlatform.DTOs
{
    public class QuizSubmitResponseDto
    {
        public int Score { get; set; }
        public int Total { get; set; }
        public int Percentage { get; set; }
        public required string Grade { get; set; }
        public required string Feedback { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}
