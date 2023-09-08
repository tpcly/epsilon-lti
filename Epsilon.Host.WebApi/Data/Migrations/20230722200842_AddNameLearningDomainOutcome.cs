using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epsilon.Host.WebApi.Data.Migrations
{
    public partial class AddNameLearningDomainOutcome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LearningDomainOutcomes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "LearningDomainOutcomes");
        }
    }
}
