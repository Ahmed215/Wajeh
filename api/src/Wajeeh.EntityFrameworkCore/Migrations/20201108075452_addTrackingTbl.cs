using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wajeeh.Migrations
{
    public partial class addTrackingTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Value",
                table: "Coupons",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "TrackingTrips",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DriverId = table.Column<long>(nullable: false),
                    RequestId = table.Column<long>(nullable: false),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingTrips_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TrackingTrips_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTrips_DriverId",
                table: "TrackingTrips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTrips_RequestId",
                table: "TrackingTrips",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackingTrips");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Coupons");
        }
    }
}
