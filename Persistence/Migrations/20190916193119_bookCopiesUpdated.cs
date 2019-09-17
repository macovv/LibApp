using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class bookCopiesUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UsersId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Books",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_UsersId",
                table: "Books",
                newName: "IX_Books_AppUserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Copy",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_AppUserId",
                table: "Books",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_AppUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Copy");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Books",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_AppUserId",
                table: "Books",
                newName: "IX_Books_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UsersId",
                table: "Books",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
