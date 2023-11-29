using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatChit.Migrations
{
    public partial class updateMessageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Messages",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "Messages");
        }
    }
}
