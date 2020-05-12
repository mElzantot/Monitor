using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class updateDepartmentFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "040941d9-95fa-4b0b-a9ef-92fed95575c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a1cfc5c-3132-4e14-b072-df905dd74672");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e309d7a-1442-4bde-af9c-706faff2bd38");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85bf3a0b-5e6a-4609-b5eb-8317e0f53d15");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb4bb29f-cc50-443e-b75a-d63a8cd48b55");

            migrationBuilder.AlterColumn<string>(
                name: "FK_ManagerID",
                table: "Departments",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52495479-f7c6-495a-98ff-bc4ad9e09a9f", "d0f3c1c1-afd6-49d5-9030-3740ba96ff1b", "Admin", "ADMIN" },
                    { "4e798ab8-3653-40fa-a131-7340d82e67f4", "b1e600f9-b774-4033-9440-8c1f4b1ace94", "Project Manager", "PROJECT MANAGER" },
                    { "007b3004-1e43-4c78-9e66-44ff3298d5a6", "ccf5d1ca-8c8d-4338-b4bb-f3d739e5eda3", "Department Manager", "DEPARTMENT MANAGER" },
                    { "5aeaf904-7407-4fb9-87f8-f6fbb1c58a5d", "89874493-e1c3-4196-8583-0dcccfb31b5f", "Team Leader", "TEAM LEADER" },
                    { "4ac739bd-65b1-4768-9f3f-1fb5b56dafaf", "db7b11d6-4987-496a-8799-2006af68ea32", "Engineer", "ENGINEER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "007b3004-1e43-4c78-9e66-44ff3298d5a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ac739bd-65b1-4768-9f3f-1fb5b56dafaf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e798ab8-3653-40fa-a131-7340d82e67f4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52495479-f7c6-495a-98ff-bc4ad9e09a9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5aeaf904-7407-4fb9-87f8-f6fbb1c58a5d");

            migrationBuilder.AlterColumn<string>(
                name: "FK_ManagerID",
                table: "Departments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "040941d9-95fa-4b0b-a9ef-92fed95575c8", "5e021b90-b6ac-4a65-8099-257a489e1fd4", "Admin", "ADMIN" },
                    { "85bf3a0b-5e6a-4609-b5eb-8317e0f53d15", "ff1ddc8d-4380-4e22-883c-3c8bf3c709ee", "Project Manager", "PROJECT MANAGER" },
                    { "1a1cfc5c-3132-4e14-b072-df905dd74672", "85db2d10-a1f5-483d-9597-e00df510a2a8", "Department Manager", "DEPARTMENT MANAGER" },
                    { "eb4bb29f-cc50-443e-b75a-d63a8cd48b55", "071ed50c-ff3a-4ece-a3d2-7a32d31b936d", "Team Leader", "TEAM LEADER" },
                    { "6e309d7a-1442-4bde-af9c-706faff2bd38", "f0bcb31a-a7da-44d5-bd9b-5905615c35a8", "Engineer", "ENGINEER" }
                });
        }
    }
}
