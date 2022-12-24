using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetAPIExample.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 12, 24, 0, 9, 14, 645, DateTimeKind.Utc).AddTicks(8825));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2022, 12, 24, 0, 9, 14, 645, DateTimeKind.Utc).AddTicks(8828));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 12, 23, 22, 55, 15, 757, DateTimeKind.Utc).AddTicks(6619));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2022, 12, 23, 22, 55, 15, 757, DateTimeKind.Utc).AddTicks(6622));
        }
    }
}
