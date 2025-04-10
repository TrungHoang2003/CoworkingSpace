using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Space_AspNetUsers_UserId",
                table: "Space");

            migrationBuilder.DropTable(
                name: "SpaceObservedHoliday");

            migrationBuilder.DropIndex(
                name: "IX_Space_UserId",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Space");

            migrationBuilder.CreateTable(
                name: "VenueHoliday",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    HolidayId = table.Column<int>(type: "int", nullable: false),
                    IsObserved = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueHoliday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VenueHoliday_Venue_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venue",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_VenueHoliday_VenueId",
                table: "VenueHoliday",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VenueHoliday");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Space",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpaceObservedHoliday",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HolidayId = table.Column<int>(type: "int", nullable: false),
                    SpaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceObservedHoliday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceObservedHoliday_Space_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Space",
                        principalColumn: "SpaceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Space_UserId",
                table: "Space",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceObservedHoliday_SpaceId",
                table: "SpaceObservedHoliday",
                column: "SpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Space_AspNetUsers_UserId",
                table: "Space",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
