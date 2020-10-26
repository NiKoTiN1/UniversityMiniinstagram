using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityMiniinstagram.Database.Migrations
{
    public partial class PostId_in_Report_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_PostId",
                table: "Reports");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "Reports",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId",
                table: "Reports",
                column: "PostId",
                unique: true,
                filter: "[PostId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_PostId",
                table: "Reports");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId",
                table: "Reports",
                column: "PostId",
                unique: true);
        }
    }
}
