using Microsoft.EntityFrameworkCore.Migrations;

namespace CrocoLanding.Model.Migrations
{
    public partial class EccSchemaLinkCatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLinkCatchRedirects_EmailLinkCatches_EmailLinkCatchId",
                table: "EmailLinkCatchRedirects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailLinkCatchRedirects",
                table: "EmailLinkCatchRedirects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailLinkCatches",
                table: "EmailLinkCatches");

            migrationBuilder.RenameTable(
                name: "EmailLinkCatchRedirects",
                newName: "EmailLinkCatchRedirect",
                newSchema: "Ecc");

            migrationBuilder.RenameTable(
                name: "EmailLinkCatches",
                newName: "EmailLinkCatch",
                newSchema: "Ecc");

            migrationBuilder.RenameIndex(
                name: "IX_EmailLinkCatchRedirects_EmailLinkCatchId",
                schema: "Ecc",
                table: "EmailLinkCatchRedirect",
                newName: "IX_EmailLinkCatchRedirect_EmailLinkCatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailLinkCatchRedirect",
                schema: "Ecc",
                table: "EmailLinkCatchRedirect",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailLinkCatch",
                schema: "Ecc",
                table: "EmailLinkCatch",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLinkCatchRedirect_EmailLinkCatch_EmailLinkCatchId",
                schema: "Ecc",
                table: "EmailLinkCatchRedirect",
                column: "EmailLinkCatchId",
                principalSchema: "Ecc",
                principalTable: "EmailLinkCatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLinkCatchRedirect_EmailLinkCatch_EmailLinkCatchId",
                schema: "Ecc",
                table: "EmailLinkCatchRedirect");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailLinkCatchRedirect",
                schema: "Ecc",
                table: "EmailLinkCatchRedirect");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailLinkCatch",
                schema: "Ecc",
                table: "EmailLinkCatch");

            migrationBuilder.RenameTable(
                name: "EmailLinkCatchRedirect",
                schema: "Ecc",
                newName: "EmailLinkCatchRedirects");

            migrationBuilder.RenameTable(
                name: "EmailLinkCatch",
                schema: "Ecc",
                newName: "EmailLinkCatches");

            migrationBuilder.RenameIndex(
                name: "IX_EmailLinkCatchRedirect_EmailLinkCatchId",
                table: "EmailLinkCatchRedirects",
                newName: "IX_EmailLinkCatchRedirects_EmailLinkCatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailLinkCatchRedirects",
                table: "EmailLinkCatchRedirects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailLinkCatches",
                table: "EmailLinkCatches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLinkCatchRedirects_EmailLinkCatches_EmailLinkCatchId",
                table: "EmailLinkCatchRedirects",
                column: "EmailLinkCatchId",
                principalTable: "EmailLinkCatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
