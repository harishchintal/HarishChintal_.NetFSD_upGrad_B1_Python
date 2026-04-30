using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FullStackELearningPlatform.Data;
using FullStackELearningPlatform.DTOs;
using FullStackELearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;

namespace FullStackELearningPlatform.Controllers
{
    [ApiController]
    [Route("api/quizzes")]
    public class QuizzesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public QuizzesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{courseId}")]
        public IActionResult GetByCourse(int courseId)
        {
            var quizzes = _context.Quizzes.AsNoTracking()
                .Where(q => q.CourseId == courseId)
                .ToList();

            return Ok(_mapper.Map<List<QuizDto>>(quizzes));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] QuizCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quiz = new Quiz
            {
                CourseId = dto.CourseId,
                Title = dto.Title
            };

            _context.Quizzes.Add(quiz);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetByCourse), new { courseId = quiz.CourseId }, _mapper.Map<QuizDto>(quiz));
        }

        [HttpGet("{quizId}/questions")]
        public IActionResult GetQuestions(int quizId)
        {
            var questions = _context.Questions.AsNoTracking()
                .Where(q => q.QuizId == quizId)
                .ToList();

            return Ok(_mapper.Map<List<QuestionResponseDto>>(questions));
        }

        [HttpPost("{quizId}/submit")]
        [Authorize]
        public IActionResult Submit(int quizId, [FromBody] QuizSubmitDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quiz = _context.Quizzes.AsNoTracking().FirstOrDefault(q => q.QuizId == quizId);
            if (quiz == null)
                return NotFound(new { message = "Quiz not found" });

            if (!_context.Users.AsNoTracking().Any(u => u.UserId == dto.UserId))
                return BadRequest(new { message = "Invalid UserId" });

            var questions = _context.Questions.AsNoTracking()
                .Where(q => q.QuizId == quizId)
                .ToList();

            if (!questions.Any())
                return NotFound(new { message = "No questions found for this quiz" });

            if (dto.Answers == null || dto.Answers.Count != questions.Count)
                return BadRequest(new { message = "Answers count does not match questions count" });

            int score = 0;
            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i].CorrectAnswer == dto.Answers[i])
                    score++;
            }

            int percentage = (score * 100) / questions.Count;
            var grade = percentage >= 80 ? "A" : percentage >= 60 ? "B" : "C";
            var feedback = percentage >= 80 ? "Excellent work!" : percentage >= 60 ? "Good job!" : "Keep practicing to improve.";
            var attemptDate = DateTime.UtcNow;

            var result = new Result
            {
                UserId = dto.UserId,
                QuizId = quizId,
                CourseId = quiz.CourseId,
                Score = score,
                AttemptDate = attemptDate
            };

            _context.Results.Add(result);
            _context.SaveChanges();

            return Ok(new QuizSubmitResponseDto
            {
                Score = score,
                Total = questions.Count,
                Percentage = percentage,
                Grade = grade,
                Feedback = feedback,
                AttemptDate = attemptDate
            });
        }
    }
}
