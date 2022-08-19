using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog_Dries.Migrations
{
    public partial class finalchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BlogPosts");

            migrationBuilder.AddColumn<string>(
                name: "Descritption",
                table: "BlogPosts",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descritption",
                table: "BlogPosts");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
