using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class updateDates2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTaskSession_SubTask_FK_SubTaskID",
                table: "SubTaskSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTaskSession",
                table: "SubTaskSession");

            migrationBuilder.RenameTable(
                name: "SubTaskSession",
                newName: "SubTaskSessions");

            migrationBuilder.RenameIndex(
                name: "IX_SubTaskSession_FK_SubTaskID",
                table: "SubTaskSessions",
                newName: "IX_SubTaskSessions_FK_SubTaskID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTaskSessions",
                table: "SubTaskSessions",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTaskSessions_SubTask_FK_SubTaskID",
                table: "SubTaskSessions",
                column: "FK_SubTaskID",
                principalTable: "SubTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTaskSessions_SubTask_FK_SubTaskID",
                table: "SubTaskSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTaskSessions",
                table: "SubTaskSessions");

            migrationBuilder.RenameTable(
                name: "SubTaskSessions",
                newName: "SubTaskSession");

            migrationBuilder.RenameIndex(
                name: "IX_SubTaskSessions_FK_SubTaskID",
                table: "SubTaskSession",
                newName: "IX_SubTaskSession_FK_SubTaskID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTaskSession",
                table: "SubTaskSession",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTaskSession_SubTask_FK_SubTaskID",
                table: "SubTaskSession",
                column: "FK_SubTaskID",
                principalTable: "SubTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
