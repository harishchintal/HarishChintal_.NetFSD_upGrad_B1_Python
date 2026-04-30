using FullStackELearningPlatform.Models;

namespace FullStackELearningPlatform.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User> GetById(int id);
    }
}