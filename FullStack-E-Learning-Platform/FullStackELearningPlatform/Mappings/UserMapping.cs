using FullStackELearningPlatform.Models;
using FullStackELearningPlatform.DTOs;

namespace FullStackELearningPlatform.Mappings
{
    public static class UserMapping
    {
        public static User ToEntity(this UserRegisterDto dto, string hashedPassword)
        {
            return new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.Now
            };
        }
    }
}