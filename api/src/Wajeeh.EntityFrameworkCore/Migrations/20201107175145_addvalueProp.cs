using Microsoft.EntityFrameworkCore.Migrations;

namespace Wajeeh.Migrations
{
    public partial class addvalueProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Value",
                table: "Coupons",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Coupons");
        }
    }
}
