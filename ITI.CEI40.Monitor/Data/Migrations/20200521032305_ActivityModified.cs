using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class ActivityModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Estimated Duration",
                table: "Task",
                newName: "End Date");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Task",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "End Date",
                table: "Task",
                newName: "Estimated Duration");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Task",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
