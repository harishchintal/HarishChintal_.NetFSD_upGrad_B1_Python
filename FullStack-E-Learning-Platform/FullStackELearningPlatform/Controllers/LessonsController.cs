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
    [Route("api")]
    public class LessonsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LessonsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("courses/{courseId}/lessons")]
        public IActionResult GetByCourse(int courseId)
        {
            if (!_context.Courses.AsNoTracking().Any(c => c.CourseId == courseId))
                return NotFound(new { message = "Course not found" });

            var lessons = _context.Lessons.AsNoTracking()
                .Where(l => l.CourseId == courseId)
                .ToList();

            return Ok(_mapper.Map<List<LessonDto>>(lessons));
        }

        [HttpPost("lessons")]
        [Authorize]
        public IActionResult Create([FromBody] LessonCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_context.Courses.AsNoTracking().Any(c => c.CourseId == dto.CourseId))
                return BadRequest(new { message = "Invalid course" });

            var lesson = _mapper.Map<Lesson>(dto);
            _context.Lessons.Add(lesson);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetByCourse), new { courseId = lesson.CourseId }, _mapper.Map<LessonDto>(lesson));
        }

        [HttpPut("lessons/{id}")]
        [Authorize]
        public IActionResult Update(int id, [FromBody] LessonDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _context.Lessons.Find(id);
            if (existing == null)
                return NotFound(new { message = "Lesson not found" });

            existing.Title = dto.Title;
            existing.Content = dto.Content;
            existing.OrderIndex = dto.OrderIndex;
            existing.DurationHours = dto.DurationHours;

            _context.SaveChanges();

            return Ok(_mapper.Map<LessonDto>(existing));
        }

        [HttpDelete("lessons/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var lesson = _context.Lessons.Find(id);
            if (lesson == null)
                return NotFound(new { message = "Lesson not found" });

            _context.Lessons.Remove(lesson);
            _context.SaveChanges();

            return Ok(new { message = "Lesson deleted successfully" });
        }
    }
}
