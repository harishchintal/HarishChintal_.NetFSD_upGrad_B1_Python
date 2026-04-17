using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FullStackELearningPlatform.Data;
using FullStackELearningPlatform.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace FullStackELearningPlatform.Controllers
{
    [ApiController]
    [Route("api/results")]
    public class ResultsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResultsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetResults(int userId)
        {
            if (!_context.Users.AsNoTracking().Any(u => u.UserId == userId))
                return NotFound(new { message = "User not found" });

            var results = _context.Results.AsNoTracking()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.AttemptDate)
                .ToList();

            var response = results.Select(r =>
            {
                var totalQuestions = _context.Questions.AsNoTracking().Count(q => q.QuizId == r.QuizId);
                var percentage = totalQuestions > 0 ? (int)((r.Score * 100) / totalQuestions) : 0;
                var grade = percentage >= 80 ? "A" : percentage >= 60 ? "B" : "C";
                var feedback = percentage >= 80 ? "Excellent work!" : percentage >= 60 ? "Good job!" : "Keep practicing to improve.";
                var courseName = _context.Courses.AsNoTracking()
                    .Where(c => c.CourseId == r.CourseId)
                    .Select(c => c.Title)
                    .FirstOrDefault() ?? string.Empty;

                return new ResultResponseDto
                {
                    ResultId = r.ResultId,
                    UserId = r.UserId,
                    QuizId = r.QuizId,
                    CourseId = r.CourseId,
                    CourseName = courseName,
                    Score = r.Score,
                    Percentage = percentage,
                    Grade = grade,
                    Feedback = feedback,
                    AttemptDate = r.AttemptDate
                };
            }).ToList();

            return Ok(response);
        }
    }
}
