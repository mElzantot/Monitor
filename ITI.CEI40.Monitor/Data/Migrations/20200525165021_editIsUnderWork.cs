using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Migrations
{
    public partial class editIsUnderWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsUnderWork",
                table: "SubTask",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsUnderWork",
                table: "SubTask",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
