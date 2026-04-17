using System.Text.Json.Serialization;

namespace FullStackELearningPlatform.Models
{
    public class Result
    {
        public int ResultId { get; set; }

        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int CourseId { get; set; }   // ✅ ADD THIS

        public int Score { get; set; }
        public DateTime AttemptDate { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Quiz? Quiz { get; set; }
        [JsonIgnore]
        public Course? Course { get; set; }
    }
}