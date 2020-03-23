using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrocoLanding.Model.Migrations
{
    public partial class EccUrlSubstitutor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailLinkCatches",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLinkCatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLinkCatchRedirects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailLinkCatchId = table.Column<string>(nullable: true),
                    RedirectedOnUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLinkCatchRedirects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailLinkCatchRedirects_EmailLinkCatches_EmailLinkCatchId",
                        column: x => x.EmailLinkCatchId,
                        principalTable: "EmailLinkCatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLinkCatchRedirects_EmailLinkCatchId",
                table: "EmailLinkCatchRedirects",
                column: "EmailLinkCatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLinkCatchRedirects");

            migrationBuilder.DropTable(
                name: "EmailLinkCatches");
        }
    }
}
