using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateHoliday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.HolidayId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_VenueHoliday_HolidayId",
                table: "VenueHoliday",
                column: "HolidayId");

            migrationBuilder.AddForeignKey(
                name: "FK_VenueHoliday_Holiday_HolidayId",
                table: "VenueHoliday",
                column: "HolidayId",
                principalTable: "Holiday",
                principalColumn: "HolidayId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VenueHoliday_Holiday_HolidayId",
                table: "VenueHoliday");

            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropIndex(
                name: "IX_VenueHoliday_HolidayId",
                table: "VenueHoliday");
        }
    }
}
