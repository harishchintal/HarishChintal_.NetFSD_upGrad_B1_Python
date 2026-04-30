using System.Text.Json.Serialization;

namespace FullStackELearningPlatform.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }

        public required string Title { get; set; }
        public required string Content { get; set; }

        public int OrderIndex { get; set; }

        public int DurationHours { get; set; }   // 🔥 ADD THIS

        [JsonIgnore]
        public Course? Course { get; set; }
    }
}