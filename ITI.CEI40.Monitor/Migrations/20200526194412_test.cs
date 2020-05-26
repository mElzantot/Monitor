using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Progress",
                table: "Project",
                nullable: true);
        }
    }
}
