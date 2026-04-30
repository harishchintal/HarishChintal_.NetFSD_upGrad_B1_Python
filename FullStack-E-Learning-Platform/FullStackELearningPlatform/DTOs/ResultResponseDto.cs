namespace FullStackELearningPlatform.DTOs
{
    public class ResultResponseDto
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public required string CourseName { get; set; }
        public int Score { get; set; }
        public int Percentage { get; set; }
        public required string Grade { get; set; }
        public required string Feedback { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}
