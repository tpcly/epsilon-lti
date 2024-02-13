using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epsilon.Host.WebApi.Data.Migrations
{
    public partial class AddLearningDomains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningDomainTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShortName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HexColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningDomainTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningDomainTypeSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningDomainTypeSet", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningDomainOutcomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RowId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColumnId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValueId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningDomainOutcomes", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_LearningDomainOutcomes_LearningDomainTypes_ColumnId",
                        column: x => x.ColumnId,
                        principalTable: "LearningDomainTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LearningDomainOutcomes_LearningDomainTypes_RowId",
                        column: x => x.RowId,
                        principalTable: "LearningDomainTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LearningDomainOutcomes_LearningDomainTypes_ValueId",
                        column: x => x.ValueId,
                        principalTable: "LearningDomainTypes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningDomains",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RowsSetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ColumnsSetId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ValuesSetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningDomains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningDomains_LearningDomainTypeSet_ColumnsSetId",
                        column: x => x.ColumnsSetId,
                        principalTable: "LearningDomainTypeSet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LearningDomains_LearningDomainTypeSet_RowsSetId",
                        column: x => x.RowsSetId,
                        principalTable: "LearningDomainTypeSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningDomains_LearningDomainTypeSet_ValuesSetId",
                        column: x => x.ValuesSetId,
                        principalTable: "LearningDomainTypeSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningDomainTypeSetTypes",
                columns: table => new
                {
                    SetsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TypesId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningDomainTypeSetTypes", x => new { x.SetsId, x.TypesId });
                    table.ForeignKey(
                        name: "FK_LearningDomainTypeSetTypes_LearningDomainTypes_TypesId",
                        column: x => x.TypesId,
                        principalTable: "LearningDomainTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningDomainTypeSetTypes_LearningDomainTypeSet_SetsId",
                        column: x => x.SetsId,
                        principalTable: "LearningDomainTypeSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LearningDomainOutcomes_ColumnId",
                table: "LearningDomainOutcomes",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningDomainOutcomes_RowId",
                table: "LearningDomainOutcomes",
                column: "RowId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningDomainOutcomes_ValueId",
                table: "LearningDomainOutcomes",
                column: "ValueId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningDomains_ColumnsSetId",
                table: "LearningDomains",
                column: "ColumnsSetId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningDomains_RowsSetId",
                table: "LearningDomains",
                column: "RowsSetId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningDomains_ValuesSetId",
                table: "LearningDomains",
                column: "ValuesSetId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningDomainTypeSetTypes_TypesId",
                table: "LearningDomainTypeSetTypes",
                column: "TypesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningDomainOutcomes");

            migrationBuilder.DropTable(
                name: "LearningDomains");

            migrationBuilder.DropTable(
                name: "LearningDomainTypeSetTypes");

            migrationBuilder.DropTable(
                name: "LearningDomainTypes");

            migrationBuilder.DropTable(
                name: "LearningDomainTypeSet");
        }
    }
}
