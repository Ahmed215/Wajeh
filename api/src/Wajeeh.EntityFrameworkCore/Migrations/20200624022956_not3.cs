using Microsoft.EntityFrameworkCore.Migrations;

namespace Wajeeh.Migrations
{
    public partial class not3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverNotifications_Drivers_DriverId",
                table: "DriverNotifications");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverNotifications_AbpUsers_DriverId",
                table: "DriverNotifications",
                column: "DriverId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverNotifications_AbpUsers_DriverId",
                table: "DriverNotifications");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverNotifications_Drivers_DriverId",
                table: "DriverNotifications",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
