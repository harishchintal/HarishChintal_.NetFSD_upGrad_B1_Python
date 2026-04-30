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
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CoursesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= GET ALL =================
        [HttpGet]
        public IActionResult GetCourses()
        {
            var courses = _context.Courses
                .AsNoTracking()
                .Include(c => c.Lessons)
                .Include(c => c.Quizzes)
                .ToList();

            return Ok(_mapper.Map<List<CourseDetailDto>>(courses));
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var course = _context.Courses
                .AsNoTracking()
                .Include(c => c.Lessons)
                .Include(c => c.Quizzes)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null)
                return NotFound(new { message = "Course not found" });

            return Ok(_mapper.Map<CourseDetailDto>(course));
        }

        // ================= CREATE =================
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] CourseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = _mapper.Map<Course>(dto);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = course.CourseId },
                _mapper.Map<CourseDetailDto>(course));
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CourseDto dto)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            _mapper.Map(dto, course);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Course updated successfully"
            });
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Course deleted successfully"
            });
        }

        // ================= COMPLETE COURSE =================
        [HttpPost("complete")]
        [Authorize]
        public async Task<IActionResult> CompleteCourseAsync([FromBody] CompleteCourseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_context.Users.AsNoTracking().Any(u => u.UserId == dto.UserId))
                return BadRequest(new { message = "Invalid UserId" });

            if (!_context.Courses.AsNoTracking().Any(c => c.CourseId == dto.CourseId))
                return BadRequest(new { message = "Invalid CourseId" });

            var alreadyCompleted = _context.Results.AsNoTracking()
                .Any(r => r.UserId == dto.UserId && r.CourseId == dto.CourseId);

            if (alreadyCompleted)
                return Ok(new { message = "Already completed" });

            var quiz = _context.Quizzes.AsNoTracking().FirstOrDefault(q => q.CourseId == dto.CourseId);
            if (quiz == null)
                return BadRequest(new { message = "No quiz found for this course" });

            var result = new Result
            {
                UserId = dto.UserId,
                CourseId = dto.CourseId,
                QuizId = quiz.QuizId,
                Score = 100,
                AttemptDate = DateTime.UtcNow
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Course completed successfully" });
        }
    }
}