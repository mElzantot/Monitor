using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class SeedDatatoDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
