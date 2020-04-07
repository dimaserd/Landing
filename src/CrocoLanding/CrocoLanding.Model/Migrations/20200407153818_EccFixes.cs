using Microsoft.EntityFrameworkCore.Migrations;

namespace CrocoLanding.Model.Migrations
{
    public partial class EccFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailInEmailGroupRelation_EmailGroup_EmailGroupId",
                schema: "Ecc",
                table: "EmailInEmailGroupRelation");

            migrationBuilder.AlterColumn<string>(
                name: "EmailGroupId",
                schema: "Ecc",
                table: "EmailInEmailGroupRelation",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailGroup_Name",
                schema: "Ecc",
                table: "EmailGroup",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailInEmailGroupRelation_EmailGroup_EmailGroupId",
                schema: "Ecc",
                table: "EmailInEmailGroupRelation",
                column: "EmailGroupId",
                principalSchema: "Ecc",
                principalTable: "EmailGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailInEmailGroupRelation_EmailGroup_EmailGroupId",
                schema: "Ecc",
                table: "EmailInEmailGroupRelation");

            migrationBuilder.DropIndex(
                name: "IX_EmailGroup_Name",
                schema: "Ecc",
                table: "EmailGroup");

            migrationBuilder.AlterColumn<string>(
                name: "EmailGroupId",
                schema: "Ecc",
                table: "EmailInEmailGroupRelation",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_EmailInEmailGroupRelation_EmailGroup_EmailGroupId",
                schema: "Ecc",
                table: "EmailInEmailGroupRelation",
                column: "EmailGroupId",
                principalSchema: "Ecc",
                principalTable: "EmailGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
