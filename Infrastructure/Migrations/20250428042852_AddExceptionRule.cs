using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExceptionRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExceptionId",
                table: "Space",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExceptionRule",
                columns: table => new
                {
                    ExceptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Days = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionRule", x => x.ExceptionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Space_ExceptionId",
                table: "Space",
                column: "ExceptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Space_ExceptionRule_ExceptionId",
                table: "Space",
                column: "ExceptionId",
                principalTable: "ExceptionRule",
                principalColumn: "ExceptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Space_ExceptionRule_ExceptionId",
                table: "Space");

            migrationBuilder.DropTable(
                name: "ExceptionRule");

            migrationBuilder.DropIndex(
                name: "IX_Space_ExceptionId",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "ExceptionId",
                table: "Space");
        }
    }
}
