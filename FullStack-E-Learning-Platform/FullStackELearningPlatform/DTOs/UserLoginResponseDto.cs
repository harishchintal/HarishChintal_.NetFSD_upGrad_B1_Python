namespace FullStackELearningPlatform.DTOs
{
    public class UserLoginResponseDto
    {
        public required int UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}