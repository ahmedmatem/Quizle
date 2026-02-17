using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizle.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOnDeleteBehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_StudentId",
                table: "QuizAttempts");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "SchoolGroups",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGroups_TeacherId",
                table: "SchoolGroups",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedByUserId",
                table: "Questions",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedByUserId",
                table: "Questions",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_StudentId",
                table: "QuizAttempts",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGroups_AspNetUsers_TeacherId",
                table: "SchoolGroups",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedByUserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_StudentId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGroups_AspNetUsers_TeacherId",
                table: "SchoolGroups");

            migrationBuilder.DropIndex(
                name: "IX_SchoolGroups_TeacherId",
                table: "SchoolGroups");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatedByUserId",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "SchoolGroups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_StudentId",
                table: "QuizAttempts",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
