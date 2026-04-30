using FullStackELearningPlatform.Data;
using FullStackELearningPlatform.DTOs;
using FullStackELearningPlatform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using FullStackELearningPlatform.Services.Implementations;
using FullStackELearningPlatform.Services.Interfaces;
using FullStackELearningPlatform.Repositories.Interfaces;

namespace FullStackELearningPlatform.Tests;

public class UserServiceTests
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockContext = new Mock<AppDbContext>();
        _mockConfig = new Mock<IConfiguration>();
        _service = new UserService(_mockContext.Object, _mockConfig.Object);
    }

    [Fact]
    public async Task Register_ValidDto_ReturnsUser()
    {
        // Arrange
        var dto = new UserRegisterDto { FullName = "Test User", Email = "test@example.com", Password = "password123" };
        _mockContext.Setup(c => c.Users.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<FullStackELearningPlatform.Models.User, bool>>>(), default)).ReturnsAsync(false);
        _mockContext.Setup(c => c.Users.Add(It.IsAny<FullStackELearningPlatform.Models.User>()));
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _service.Register(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.FullName, result.FullName);
        Assert.Equal(dto.Email, result.Email);
    }

    [Fact]
    public async Task Register_EmailAlreadyExists_ThrowsException()
    {
        // Arrange
        var dto = new UserRegisterDto { FullName = "Test User", Email = "test@example.com", Password = "password123" };
        _mockContext.Setup(c => c.Users.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<FullStackELearningPlatform.Models.User, bool>>>(), default)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Register(dto));
    }
}

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _mockRepo;
    private readonly CourseService _service;

    public CourseServiceTests()
    {
        _mockRepo = new Mock<ICourseRepository>();
        _service = new CourseService(_mockRepo.Object);
    }

    [Fact]
    public void GetCourses_ReturnsList()
    {
        // Arrange
        var courses = new List<FullStackELearningPlatform.Models.Course>
        {
            new FullStackELearningPlatform.Models.Course { CourseId = 1, Title = "Test Course", Description = "Test Description", CreatedBy = 1 }
        };
        _mockRepo.Setup(r => r.GetAll()).Returns(courses);

        // Act
        var result = _service.GetCourses();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Course", result[0].Title);
    }
}