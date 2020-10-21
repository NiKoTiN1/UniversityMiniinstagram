using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityMiniinstagram.Database.Migrations
{
    public partial class Categoryinpost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "category",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "category",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
