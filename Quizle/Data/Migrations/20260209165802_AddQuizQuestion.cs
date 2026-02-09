using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizle.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_SchoolGroup_GroupId",
                table: "GroupStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolGroup",
                table: "SchoolGroup");

            migrationBuilder.RenameTable(
                name: "SchoolGroup",
                newName: "SchoolGroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolGroups",
                table: "SchoolGroups",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActiveFromUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_SchoolGroups_SchoolGroupId",
                        column: x => x.SchoolGroupId,
                        principalTable: "SchoolGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    CorrectNumeric = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumericTolerance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CorrectOptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_SchoolGroupId",
                table: "Quizzes",
                column: "SchoolGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_SchoolGroups_GroupId",
                table: "GroupStudents",
                column: "GroupId",
                principalTable: "SchoolGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_SchoolGroups_GroupId",
                table: "GroupStudents");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolGroups",
                table: "SchoolGroups");

            migrationBuilder.RenameTable(
                name: "SchoolGroups",
                newName: "SchoolGroup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolGroup",
                table: "SchoolGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_SchoolGroup_GroupId",
                table: "GroupStudents",
                column: "GroupId",
                principalTable: "SchoolGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
