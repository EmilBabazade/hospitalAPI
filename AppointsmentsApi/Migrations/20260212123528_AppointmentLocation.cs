using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointsmentsApi.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location_Building",
                table: "Appointments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location_RoomNumber",
                table: "Appointments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Building",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Location_RoomNumber",
                table: "Appointments");
        }
    }
}
