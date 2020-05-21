using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class updateDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EngineerSubTasks");

            migrationBuilder.RenameColumn(
                name: "Estimated Duration",
                table: "SubTask",
                newName: "End Date");

            migrationBuilder.AlterColumn<int>(
                name: "Progress",
                table: "SubTask",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<int>(
                name: "Evaluation",
                table: "SubTask",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FK_EngineerID",
                table: "SubTask",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsUnderWork",
                table: "SubTask",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SubTaskSession",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FK_SubTaskID = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(name: "Start Date", nullable: false),
                    EndDate = table.Column<DateTime>(name: "End Date", nullable: true),
                    SessDuration = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTaskSession", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubTaskSession_SubTask_FK_SubTaskID",
                        column: x => x.FK_SubTaskID,
                        principalTable: "SubTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTask_FK_EngineerID",
                table: "SubTask",
                column: "FK_EngineerID");

            migrationBuilder.CreateIndex(
                name: "IX_SubTaskSession_FK_SubTaskID",
                table: "SubTaskSession",
                column: "FK_SubTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTask_AspNetUsers_FK_EngineerID",
                table: "SubTask",
                column: "FK_EngineerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTask_AspNetUsers_FK_EngineerID",
                table: "SubTask");

            migrationBuilder.DropTable(
                name: "SubTaskSession");

            migrationBuilder.DropIndex(
                name: "IX_SubTask_FK_EngineerID",
                table: "SubTask");

            migrationBuilder.DropColumn(
                name: "Evaluation",
                table: "SubTask");

            migrationBuilder.DropColumn(
                name: "FK_EngineerID",
                table: "SubTask");

            migrationBuilder.DropColumn(
                name: "IsUnderWork",
                table: "SubTask");

            migrationBuilder.RenameColumn(
                name: "End Date",
                table: "SubTask",
                newName: "Estimated Duration");

            migrationBuilder.AlterColumn<float>(
                name: "Progress",
                table: "SubTask",
                nullable: false,
                oldClrType: typeof(int));

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
