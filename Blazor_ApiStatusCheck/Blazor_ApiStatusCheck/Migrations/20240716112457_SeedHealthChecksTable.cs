using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blazor_ApiStatusCheck.Migrations
{
    /// <inheritdoc />
    public partial class SeedHealthChecksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "HealthChecks",
                columns: new[] { "Api__Id", "CreatedAt", "LastChecked", "Message", "Name", "Status", "Url" },
                values: new object[,]
                {
                    { "1", new DateTime(2024, 7, 16, 14, 24, 56, 778, DateTimeKind.Local).AddTicks(6360), new DateTime(2024, 7, 16, 14, 24, 56, 778, DateTimeKind.Local).AddTicks(6371), "healthy", "Api_1", "healthy", "www.api1.com" },
                    { "2", new DateTime(2024, 7, 16, 14, 24, 56, 778, DateTimeKind.Local).AddTicks(6375), new DateTime(2024, 7, 16, 14, 24, 56, 778, DateTimeKind.Local).AddTicks(6376), "healthy", "Api_2", "healthy", "www.api2.com" },
                    { "3", new DateTime(2024, 7, 16, 14, 24, 56, 778, DateTimeKind.Local).AddTicks(6377), new DateTime(2024, 7, 16, 14, 24, 56, 778, DateTimeKind.Local).AddTicks(6378), "unhealthy", "Api_3", "unhealthy", "www.api3.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HealthChecks",
                keyColumn: "Api__Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "HealthChecks",
                keyColumn: "Api__Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "HealthChecks",
                keyColumn: "Api__Id",
                keyValue: "3");
        }
    }
}
