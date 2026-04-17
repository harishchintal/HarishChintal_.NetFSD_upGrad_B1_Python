using FullStackELearningPlatform.Models;

namespace FullStackELearningPlatform.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course? GetById(int id);
        void Add(Course course);
    }
}