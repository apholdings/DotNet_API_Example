using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotNetAPIExample.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Rate = table.Column<double>(type: "double precision", nullable: false),
                    Thumbnail = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogNumbers",
                columns: table => new
                {
                    BlogNo = table.Column<int>(type: "integer", nullable: false),
                    BlogID = table.Column<int>(type: "integer", nullable: false),
                    SpecialDetails = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogNumbers", x => x.BlogNo);
                    table.ForeignKey(
                        name: "FK_BlogNumbers_Blogs_BlogID",
                        column: x => x.BlogID,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Content", "CreatedDate", "Description", "Rate", "Thumbnail", "Title", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Tuve sexo con antonella en el 2023", new DateTime(2022, 12, 23, 20, 45, 44, 817, DateTimeKind.Utc).AddTicks(4010), "La vez que tuve sex con a", 200.0, "https://boomslag.s3.us-east-2.amazonaws.com/lightbulb.jpg", "Sexo con anto", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Antos titties", new DateTime(2022, 12, 23, 20, 45, 44, 817, DateTimeKind.Utc).AddTicks(4012), "Las tetas de antonella, YUMMY", 200.0, "https://boomslag.s3.us-east-2.amazonaws.com/lightbulb.jpg", "Titties", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogNumbers_BlogID",
                table: "BlogNumbers",
                column: "BlogID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogNumbers");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
