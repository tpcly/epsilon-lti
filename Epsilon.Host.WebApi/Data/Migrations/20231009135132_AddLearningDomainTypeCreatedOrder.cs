using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epsilon.Host.WebApi.Data.Migrations
{
    public partial class AddLearningDomainTypeCreatedOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomainTypes_RowId",
                table: "LearningDomainOutcomes");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomainTypes_ValueId",
                table: "LearningDomainOutcomes");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "LearningDomainTypes",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LearningDomainOutcomes",
                keyColumn: "ValueId",
                keyValue: null,
                column: "ValueId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ValueId",
                table: "LearningDomainOutcomes",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "LearningDomainOutcomes",
                keyColumn: "RowId",
                keyValue: null,
                column: "RowId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RowId",
                table: "LearningDomainOutcomes",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomainTypes_RowId",
                table: "LearningDomainOutcomes",
                column: "RowId",
                principalTable: "LearningDomainTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomainTypes_ValueId",
                table: "LearningDomainOutcomes",
                column: "ValueId",
                principalTable: "LearningDomainTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomainTypes_RowId",
                table: "LearningDomainOutcomes");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomainTypes_ValueId",
                table: "LearningDomainOutcomes");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "LearningDomainTypes");

            migrationBuilder.AlterColumn<string>(
                name: "ValueId",
                table: "LearningDomainOutcomes",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RowId",
                table: "LearningDomainOutcomes",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomainTypes_RowId",
                table: "LearningDomainOutcomes",
                column: "RowId",
                principalTable: "LearningDomainTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningDomainOutcomes_LearningDomainTypes_ValueId",
                table: "LearningDomainOutcomes",
                column: "ValueId",
                principalTable: "LearningDomainTypes",
                principalColumn: "Id");
        }
    }
}
