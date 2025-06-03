using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorbikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceListId",
                table: "Motorbike",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceListId",
                table: "Motorbike");
        }
    }
}
