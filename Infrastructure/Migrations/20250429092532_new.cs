using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "PricePerMonth",
                table: "Space");

            migrationBuilder.AddColumn<string>(
                name: "ListingType",
                table: "SpaceType",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Capacity",
                table: "Space",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Deposit",
                table: "Space",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListingType",
                table: "Space",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceId",
                table: "Space",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Space",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Space",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VirtualVideoUrl",
                table: "Space",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeUnit = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SetupFee = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Space_PriceId",
                table: "Space",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Space_Price_PriceId",
                table: "Space",
                column: "PriceId",
                principalTable: "Price",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Space_Price_PriceId",
                table: "Space");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropIndex(
                name: "IX_Space_PriceId",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "SpaceType");

            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Space");

            migrationBuilder.DropColumn(
                name: "VirtualVideoUrl",
                table: "Space");

            migrationBuilder.AlterColumn<int>(
                name: "Capacity",
                table: "Space",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerHour",
                table: "Space",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerMonth",
                table: "Space",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
