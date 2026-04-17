using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.DTOs;

namespace FullStackELearningPlatform.Mappings
{
    public static class CourseMapping
    {
        // DTO → Model
        public static Course ToEntity(this CourseDto dto)
        {
            return new Course
            {
                Title = dto.Title,
                Description = dto.Description
            };
        }

        // Model → DTO
        public static CourseDto ToDto(this Course course)
        {
            return new CourseDto
            {
                Title = course.Title,
                Description = course.Description
            };
        }
    }
}