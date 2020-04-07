using Microsoft.EntityFrameworkCore.Migrations;

namespace CrocoLanding.Model.Migrations
{
    public partial class EccMessageDistribution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageDistributionId",
                schema: "Ecc",
                table: "Interaction",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageDistribution",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageDistribution", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interaction_MessageDistributionId",
                schema: "Ecc",
                table: "Interaction",
                column: "MessageDistributionId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDistribution_Type",
                schema: "Ecc",
                table: "MessageDistribution",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageDistribution",
                schema: "Ecc");

            migrationBuilder.DropIndex(
                name: "IX_Interaction_MessageDistributionId",
                schema: "Ecc",
                table: "Interaction");

            migrationBuilder.DropColumn(
                name: "MessageDistributionId",
                schema: "Ecc",
                table: "Interaction");
        }
    }
}
