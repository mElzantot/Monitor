using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class testDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8d58a43-a764-47c0-8f79-0480f85117f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa3ca182-d0ed-4d96-9fc9-854c38719a67");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b177b33a-4b16-4e6b-9351-6720a034a2a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b37665b1-b1cc-4503-89f6-b88c2ab74be7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed6f36c9-d4a1-4189-b31b-a395d2b7a64c");

            migrationBuilder.AlterColumn<string>(
                name: "FK_TeamLeaderId",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FK_TeamLeaderId",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aa3ca182-d0ed-4d96-9fc9-854c38719a67", "854ce3f6-dc75-42b2-8a9a-72a145b39a81", "Admin", "ADMIN" },
                    { "b177b33a-4b16-4e6b-9351-6720a034a2a9", "baa46d31-4561-414e-9a49-df0da5b6be0e", "Project Manager", "PROJECT MANAGER" },
                    { "b37665b1-b1cc-4503-89f6-b88c2ab74be7", "d75bfb74-2348-4c5b-a1cd-4763ca25b009", "Department Manager", "DEPARTMENT MANAGER" },
                    { "ed6f36c9-d4a1-4189-b31b-a395d2b7a64c", "214f622d-d351-46eb-9fc0-b02b665217b4", "Team Leader", "TEAM LEADER" },
                    { "a8d58a43-a764-47c0-8f79-0480f85117f9", "23d4fe62-5a8b-4d72-9a2d-ec266377050c", "Engineer", "ENGINEER" }
                });
        }
    }
}
