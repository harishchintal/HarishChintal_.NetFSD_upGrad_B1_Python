using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.Repositories;

namespace FullStackELearningPlatform.Services
{
    public class QuizService
    {
        private readonly IQuizRepository _repo;

        public QuizService(IQuizRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> SubmitQuiz(int quizId, List<string> answers)
        {
            var questions = await _repo.GetQuestions(quizId);

            int score = 0;

            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i].CorrectAnswer == answers[i])
                    score++;
            }

            return score;
        }
    }
}