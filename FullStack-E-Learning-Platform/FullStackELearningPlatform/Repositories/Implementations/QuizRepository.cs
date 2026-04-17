using Microsoft.EntityFrameworkCore;
using FullStackELearningPlatform.Data;
using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.Repositories;

namespace FullStackELearningPlatform.Repositories.Implementations
{
    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _context;

        public QuizRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetQuestions(int quizId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId)
                .ToListAsync();
        }

        public async Task AddResult(Result result)
        {
            _context.Results.Add(result);
            await _context.SaveChangesAsync();
        }
    }
}
