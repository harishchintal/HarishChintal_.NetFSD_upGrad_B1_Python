using FullStackELearningPlatform.Models;

namespace FullStackELearningPlatform.Services.Interfaces
{
    public interface ICourseService
    {
        List<Course> GetCourses();
        Course? GetCourse(int id);
        void AddCourse(Course course);
    }
}