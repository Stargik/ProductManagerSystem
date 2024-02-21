using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedSubscriberRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76474cd1-a32e-44bb-b2da-6c77975b8b04");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c0904df-63f0-436b-a6e7-a5444e0ab7be");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b05a81e-d570-449a-a3ae-242a69ed693a", null, "user", "USER" },
                    { "53988f91-e222-41b1-9556-a11c31ca8136", null, "subscriber", "SUBSCRIBER" },
                    { "5550e56e-0ac6-434a-8e86-995de5a9ab4e", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b05a81e-d570-449a-a3ae-242a69ed693a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "53988f91-e222-41b1-9556-a11c31ca8136");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5550e56e-0ac6-434a-8e86-995de5a9ab4e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "76474cd1-a32e-44bb-b2da-6c77975b8b04", null, "user", "USER" },
                    { "8c0904df-63f0-436b-a6e7-a5444e0ab7be", null, "admin", "ADMIN" }
                });
        }
    }
}
