using System.Text.Json.Serialization;

namespace FullStackELearningPlatform.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public int CreatedBy { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public List<Lesson>? Lessons { get; set; }
        public List<Quiz>? Quizzes { get; set; }
    }
}