using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class RemoveEngineerSubTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EngineerSubTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EngineerSubTasks",
                columns: table => new
                {
                    EngineerID = table.Column<string>(nullable: false),
                    SubTaskID = table.Column<int>(nullable: false),
                    Evaluation = table.Column<float>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineerSubTasks", x => new { x.EngineerID, x.SubTaskID });
                    table.ForeignKey(
                        name: "FK_EngineerSubTasks_AspNetUsers_EngineerID",
                        column: x => x.EngineerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EngineerSubTasks_SubTask_SubTaskID",
                        column: x => x.SubTaskID,
                        principalTable: "SubTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EngineerSubTasks_SubTaskID",
                table: "EngineerSubTasks",
                column: "SubTaskID");
        }
    }
}
