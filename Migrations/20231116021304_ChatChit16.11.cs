using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatChit.Migrations
{
    public partial class ChatChit1611 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_friendId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_userId",
                table: "Friends");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Friends",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "friendId",
                table: "Friends",
                newName: "friend_id");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_userId",
                table: "Friends",
                newName: "IX_Friends_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_friendId",
                table: "Friends",
                newName: "IX_Friends_friend_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_friend_id",
                table: "Friends",
                column: "friend_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_user_id",
                table: "Friends",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_friend_id",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_user_id",
                table: "Friends");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Friends",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "friend_id",
                table: "Friends",
                newName: "friendId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_user_id",
                table: "Friends",
                newName: "IX_Friends_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_friend_id",
                table: "Friends",
                newName: "IX_Friends_friendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_friendId",
                table: "Friends",
                column: "friendId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_userId",
                table: "Friends",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
