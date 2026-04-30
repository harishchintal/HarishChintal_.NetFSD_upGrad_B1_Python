using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullStackELearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationHours",
                table: "Lessons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationHours",
                table: "Lessons");
        }
    }
}
