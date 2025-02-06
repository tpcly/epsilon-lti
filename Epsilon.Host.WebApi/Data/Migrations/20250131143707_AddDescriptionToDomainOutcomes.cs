using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epsilon.Host.WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToDomainOutcomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LearningDomainOutcomes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "LearningDomainOutcomes");
        }
    }
}
