namespace FullStackELearningPlatform.DTOs
{
    public class ProfileDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public int TotalCourses { get; set; }
        public int CompletedCourses { get; set; }
        public int LastScore { get; set; }
        public int LastPercentage { get; set; }
        public required string LastGrade { get; set; }
        public required string LastFeedback { get; set; }
        public List<string>? CompletedCoursesNames { get; set; }
    }
}
