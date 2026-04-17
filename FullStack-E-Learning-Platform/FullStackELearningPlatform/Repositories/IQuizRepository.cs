using FullStackELearningPlatform.Models;

namespace FullStackELearningPlatform.Repositories
{
    public interface IQuizRepository
    {
        Task<List<Question>> GetQuestions(int quizId);
        Task AddResult(Result result);
    }
}