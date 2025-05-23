using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorbikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalPricePerDay",
                table: "Motorbike");

            migrationBuilder.DropColumn(
                name: "RentalPricePerHour",
                table: "Motorbike");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RentalPricePerDay",
                table: "Motorbike",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RentalPricePerHour",
                table: "Motorbike",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
