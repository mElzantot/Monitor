using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
