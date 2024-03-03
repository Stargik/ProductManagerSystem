using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTypeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "CurrencyTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "CurrencyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Rate",
                value: 40m);

            migrationBuilder.UpdateData(
                table: "CurrencyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Rate",
                value: 40m);

            migrationBuilder.UpdateData(
                table: "CurrencyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Rate",
                value: 1m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "CurrencyTypes");
        }
    }
}
