using AutoMapper;
using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.DTOs;

namespace FullStackELearningPlatform.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDto>();
            CreateMap<Course, CourseDetailDto>();
            CreateMap<Lesson, LessonDto>().ReverseMap();
            CreateMap<LessonCreateDto, Lesson>();
            CreateMap<Quiz, QuizDto>().ReverseMap();
            CreateMap<Question, QuestionResponseDto>();
            CreateMap<User, UserDto>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}