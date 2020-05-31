using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Migrations
{
    public partial class ToFixBugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDuration",
                table: "SubTask");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualDuration",
                table: "SubTask",
                nullable: false,
                defaultValue: 0);
        }
    }
}
