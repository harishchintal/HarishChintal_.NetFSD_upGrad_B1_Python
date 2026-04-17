using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using FullStackELearningPlatform.Data;
using FullStackELearningPlatform.Services;
using FullStackELearningPlatform.Services.Implementations;
using FullStackELearningPlatform.Services.Interfaces;
using FullStackELearningPlatform.Repositories;
using FullStackELearningPlatform.Repositories.Interfaces;
using FullStackELearningPlatform.Repositories.Implementations;
using FullStackELearningPlatform.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured"),
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")))
        };
    });

builder.Services.AddAuthorization();

// ✅ REPOSITORIES
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();

// ✅ SERVICES
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<QuizService>();

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        p => p.WithOrigins("http://localhost:3000", "http://localhost:5000", "http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<FullStackELearningPlatform.Middleware.ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.MapControllers();

app.Run();