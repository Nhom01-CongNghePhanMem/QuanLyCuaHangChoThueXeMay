using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorbikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Customer");
        }
    }
}
