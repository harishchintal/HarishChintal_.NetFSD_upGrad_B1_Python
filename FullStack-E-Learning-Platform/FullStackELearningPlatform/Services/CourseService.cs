using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.Repositories.Interfaces;
using FullStackELearningPlatform.Services.Interfaces;

namespace FullStackELearningPlatform.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;

        public CourseService(ICourseRepository repo)
        {
            _repo = repo;
        }

        public List<Course> GetCourses()
        {
            return _repo.GetAll();
        }

        // ✅ FIXED (nullable)
        public Course? GetCourse(int id)
        {
            return _repo.GetById(id);
        }

        public void AddCourse(Course course)
        {
            _repo.Add(course);
        }
    }
}