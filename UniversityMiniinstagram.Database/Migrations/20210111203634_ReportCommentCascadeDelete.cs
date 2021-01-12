using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityMiniinstagram.Database.Migrations
{
    public partial class ReportCommentCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Comments_CommentId",
                table: "Reports");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Comments_CommentId",
                table: "Reports",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Comments_CommentId",
                table: "Reports");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Comments_CommentId",
                table: "Reports",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
