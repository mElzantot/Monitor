using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FK_TeamID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SalaryRate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalEvaluation",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Workload",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    FK_ManagerID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_AspNetUsers_FK_ManagerID",
                        column: x => x.FK_ManagerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Owner = table.Column<string>(maxLength: 30, nullable: false),
                    TotalBudget = table.Column<float>(nullable: false),
                    Income = table.Column<float>(nullable: false),
                    Outcome = table.Column<float>(nullable: false),
                    Progress = table.Column<float>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    EstimatedDuration = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    FK_TeamLeaderId = table.Column<string>(nullable: false),
                    FK_DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Departments_FK_DepartmentId",
                        column: x => x.FK_DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_AspNetUsers_FK_TeamLeaderId",
                        column: x => x.FK_TeamLeaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentProjects",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(nullable: false),
                    ProjectID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentProjects", x => new { x.DepartmentID, x.ProjectID });
                    table.ForeignKey(
                        name: "FK_DepartmentProjects_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentProjects_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    FK_TeamId = table.Column<int>(nullable: false),
                    FK_ProjectId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(name: "Start Date", nullable: false),
                    EstimatedDuration = table.Column<DateTime>(name: "Estimated Duration", nullable: false),
                    Progress = table.Column<float>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_Project_FK_ProjectId",
                        column: x => x.FK_ProjectId,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Task_Teams_FK_TeamId",
                        column: x => x.FK_TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubTask",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    FK_TaskId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(name: "Start Date", nullable: false),
                    EstimatedDuration = table.Column<DateTime>(name: "Estimated Duration", nullable: false),
                    Progress = table.Column<float>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTask_Task_FK_TaskId",
                        column: x => x.FK_TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    FK_SubTaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_SubTask_FK_SubTaskId",
                        column: x => x.FK_SubTaskId,
                        principalTable: "SubTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EngineerSubTasks",
                columns: table => new
                {
                    EngineerID = table.Column<string>(nullable: false),
                    SubTaskID = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Evaluation = table.Column<float>(nullable: false)
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
                name: "IX_AspNetUsers_FK_TeamID",
                table: "AspNetUsers",
                column: "FK_TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_FK_SubTaskId",
                table: "Claims",
                column: "FK_SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentProjects_ProjectID",
                table: "DepartmentProjects",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FK_ManagerID",
                table: "Departments",
                column: "FK_ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_EngineerSubTasks_SubTaskID",
                table: "EngineerSubTasks",
                column: "SubTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_SubTask_FK_TaskId",
                table: "SubTask",
                column: "FK_TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_FK_ProjectId",
                table: "Task",
                column: "FK_ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_FK_TeamId",
                table: "Task",
                column: "FK_TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_FK_DepartmentId",
                table: "Teams",
                column: "FK_DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_FK_TeamLeaderId",
                table: "Teams",
                column: "FK_TeamLeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_FK_TeamID",
                table: "AspNetUsers",
                column: "FK_TeamID",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_FK_TeamID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "DepartmentProjects");

            migrationBuilder.DropTable(
                name: "EngineerSubTasks");

            migrationBuilder.DropTable(
                name: "SubTask");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FK_TeamID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvailableTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FK_TeamID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SalaryRate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalEvaluation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Workload",
                table: "AspNetUsers");
        }
    }
}
