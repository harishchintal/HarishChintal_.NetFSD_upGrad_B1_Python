using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FullStackELearningPlatform.Data;
using FullStackELearningPlatform.DTOs;
using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.Services;
using Microsoft.AspNetCore.Authorization;

namespace FullStackELearningPlatform.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserService _service;
        private readonly IMapper _mapper;

        public UsersController(AppDbContext context, UserService service, IMapper mapper)
        {
            _context = context;
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _service.Register(dto);
                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, _mapper.Map<UserDto>(user));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _service.Login(dto);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/dashboard")]
        [Authorize]
        public IActionResult GetDashboard(int id)
        {
            var userExists = _context.Users.AsNoTracking().Any(u => u.UserId == id);
            if (!userExists)
                return NotFound(new { message = "User not found" });

            var totalCourses = _context.Courses.AsNoTracking().Count();
            var completedCourses = _context.Results.AsNoTracking()
                .Where(r => r.UserId == id && r.CourseId != 0)
                .Select(r => r.CourseId)
                .Distinct()
                .Count();

            int progress = totalCourses == 0 ? 0 : (completedCourses * 100) / totalCourses;

            return Ok(new DashboardDto
            {
                TotalCourses = totalCourses,
                CompletedCourses = completedCourses,
                Progress = progress
            });
        }

        [HttpGet("{id}/profile")]
        [Authorize]
        public IActionResult GetProfile(int id)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var totalCourses = _context.Courses.AsNoTracking().Count();
            var results = _context.Results.AsNoTracking()
                .Where(r => r.UserId == id)
                .OrderByDescending(r => r.AttemptDate)
                .ToList();

            var completedCourses = results.Select(r => r.CourseId).Distinct().Count();
            var completedCourseNames = _context.Courses.AsNoTracking()
                .Where(c => results.Select(r => r.CourseId).Distinct().Contains(c.CourseId))
                .Select(c => c.Title)
                .ToList();

            var lastResult = results.FirstOrDefault();
            int lastPercentage = 0;
            string lastGrade = "N/A";
            string lastFeedback = "No quiz attempts yet.";

            if (lastResult != null)
            {
                var totalQuestions = _context.Questions.AsNoTracking().Count(q => q.QuizId == lastResult.QuizId);
                if (totalQuestions > 0)
                {
                    lastPercentage = (int)((lastResult.Score * 100) / totalQuestions);
                }
                lastGrade = lastPercentage >= 80 ? "A" : lastPercentage >= 60 ? "B" : "C";
                lastFeedback = lastPercentage >= 80 ? "Excellent work!" : lastPercentage >= 60 ? "Good job!" : "Keep practicing to improve.";
            }

            return Ok(new ProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                TotalCourses = totalCourses,
                CompletedCourses = completedCourses,
                LastScore = lastResult?.Score ?? 0,
                LastPercentage = lastPercentage,
                LastGrade = lastGrade,
                LastFeedback = lastFeedback,
                CompletedCoursesNames = completedCourseNames
            });
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.AsNoTracking().ToList();
            return Ok(_mapper.Map<List<UserDto>>(users));
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _context.Users.Find(id);
            if (existing == null)
                return NotFound(new { message = "User not found" });

            existing.FullName = dto.FullName;
            existing.Email = dto.Email;
            _context.SaveChanges();

            return Ok(_mapper.Map<UserDto>(existing));
        }
    }
}
