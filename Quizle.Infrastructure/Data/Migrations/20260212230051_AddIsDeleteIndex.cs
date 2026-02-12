using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizle.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeleteIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "SchoolGroups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SchoolGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "ChoiceOptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChoiceOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_IsDeleted",
                table: "Quizzes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_IsDeleted",
                table: "Questions",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizzes_IsDeleted",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Questions_IsDeleted",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "SchoolGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SchoolGroups");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "ChoiceOptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChoiceOptions");
        }
    }
}
