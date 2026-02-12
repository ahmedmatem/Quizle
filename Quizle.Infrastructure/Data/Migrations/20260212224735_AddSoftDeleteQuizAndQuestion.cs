using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizle.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteQuizAndQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Quizzes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Questions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Questions");
        }
    }
}
