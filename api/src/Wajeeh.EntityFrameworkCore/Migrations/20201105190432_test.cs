using Microsoft.EntityFrameworkCore.Migrations;

namespace Wajeeh.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackVehiclePicture",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "FrontVehiclePicture",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "IdentityPicture",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LisencePicture",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "VehicleLisencePicture",
                table: "Drivers");

            migrationBuilder.AddColumn<int>(
                name: "AddressTitle",
                table: "Drivers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DateOfBirthGregorian",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Long",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Drivers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressTitle",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DateOfBirthGregorian",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Long",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "BackVehiclePicture",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontVehiclePicture",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityPicture",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LisencePicture",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleLisencePicture",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
