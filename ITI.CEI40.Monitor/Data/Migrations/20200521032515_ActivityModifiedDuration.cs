using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class ActivityModifiedDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Estimated Duration",
                table: "Task",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estimated Duration",
                table: "Task");
        }
    }
}
