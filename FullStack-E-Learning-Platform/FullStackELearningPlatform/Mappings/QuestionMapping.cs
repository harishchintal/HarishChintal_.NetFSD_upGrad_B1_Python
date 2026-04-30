using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.DTOs;

namespace FullStackELearningPlatform.Mappings
{
    public static class QuestionMapping
    {
        public static Question ToEntity(this QuestionDto dto)
        {
            return new Question
            {
                QuizId = dto.QuizId,
                QuestionText = dto.QuestionText,
                OptionA = dto.OptionA,
                OptionB = dto.OptionB,
                OptionC = dto.OptionC,
                OptionD = dto.OptionD,
                CorrectAnswer = dto.CorrectAnswer
            };
        }
    }
}