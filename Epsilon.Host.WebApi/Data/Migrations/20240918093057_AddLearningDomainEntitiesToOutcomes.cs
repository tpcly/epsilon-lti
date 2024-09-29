using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epsilon.Host.WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLearningDomainEntitiesToOutcomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LearningDomainOutcomes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "DomainId",
                table: "LearningDomainOutcomes",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LearningDomainOutcomes_DomainId",
                table: "LearningDomainOutcomes",
                column: "DomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomains_DomainId",
                table: "LearningDomainOutcomes",
                column: "DomainId",
                principalTable: "LearningDomains",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomains_DomainId",
                table: "LearningDomainOutcomes");

            migrationBuilder.DropIndex(
                name: "IX_LearningDomainOutcomes_DomainId",
                table: "LearningDomainOutcomes");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "LearningDomainOutcomes");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LearningDomainOutcomes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
