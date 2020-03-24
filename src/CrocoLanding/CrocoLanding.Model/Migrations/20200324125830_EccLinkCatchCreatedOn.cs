using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrocoLanding.Model.Migrations
{
    public partial class EccLinkCatchCreatedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "EmailLinkCatches",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "EmailLinkCatches");
        }
    }
}
