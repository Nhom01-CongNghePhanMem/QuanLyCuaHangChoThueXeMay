using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorbikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableRentalContract_Motorbike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MotorbikeId",
                table: "RentalContract",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_RentalContract_MotorbikeId",
                table: "RentalContract",
                column: "MotorbikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContract_Motorbike_MotorbikeId",
                table: "RentalContract",
                column: "MotorbikeId",
                principalTable: "Motorbike",
                principalColumn: "MotorbikeId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalContract_Motorbike_MotorbikeId",
                table: "RentalContract");

            migrationBuilder.DropIndex(
                name: "IX_RentalContract_MotorbikeId",
                table: "RentalContract");

            migrationBuilder.AlterColumn<int>(
                name: "MotorbikeId",
                table: "RentalContract",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
