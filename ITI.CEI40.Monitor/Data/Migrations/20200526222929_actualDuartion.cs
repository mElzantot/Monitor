﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Migrations
{
    public partial class actualDuartion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualDuratin",
                table: "Task",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SessDuration",
                table: "SubTaskSessions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActualDuration",
                table: "SubTask",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDuratin",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "ActualDuration",
                table: "SubTask");

            migrationBuilder.AlterColumn<int>(
                name: "SessDuration",
                table: "SubTaskSessions",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
