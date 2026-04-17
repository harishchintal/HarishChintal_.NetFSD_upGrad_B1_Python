using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.DTOs;

namespace FullStackELearningPlatform.Mappings
{
    public static class QuizMapping
    {
        public static Quiz ToEntity(this QuizCreateDto dto)
        {
            return new Quiz
            {
                CourseId = dto.CourseId,
                Title = dto.Title
            };
        }
    }
}