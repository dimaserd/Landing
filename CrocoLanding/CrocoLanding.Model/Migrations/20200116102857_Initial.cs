using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrocoLanding.Model.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Store");

            migrationBuilder.CreateTable(
                name: "WebAppRequestContextLogs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    RequestId = table.Column<string>(nullable: true),
                    ParentRequestId = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(nullable: true),
                    StartedOn = table.Column<DateTime>(nullable: false),
                    FinishedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebAppRequestContextLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EntityName = table.Column<string>(nullable: true),
                    OperatedAt = table.Column<DateTime>(nullable: false),
                    OperatedBy = table.Column<string>(nullable: true),
                    KeyValues = table.Column<string>(nullable: true),
                    OldValues = table.Column<string>(nullable: true),
                    NewValues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationMessageLog",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MessageType = table.Column<string>(nullable: true),
                    MessageJson = table.Column<string>(nullable: true),
                    RequestId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationMessageLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoggedApplicationAction",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<string>(maxLength: 128, nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsException = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ParametersJson = table.Column<string>(nullable: true),
                    ExceptionStackTrace = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    SeverityType = table.Column<int>(nullable: false),
                    TransactionId = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggedApplicationAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoggedUserInterfaceAction",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDate = table.Column<DateTime>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsException = table.Column<bool>(nullable: false),
                    EventId = table.Column<string>(maxLength: 128, nullable: true),
                    ParametersJson = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggedUserInterfaceAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RobotTask",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Script = table.Column<string>(nullable: true),
                    Result = table.Column<int>(nullable: false),
                    IsExecutionDelayed = table.Column<bool>(nullable: false),
                    ToExecuteOn = table.Column<DateTime>(nullable: false),
                    StartedOn = table.Column<DateTime>(nullable: true),
                    ExecutedOn = table.Column<DateTime>(nullable: true),
                    ExceptionStackTrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobotTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationMessageStatusLog",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MessageId = table.Column<string>(nullable: true),
                    StartedOn = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationMessageStatusLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationMessageStatusLog_IntegrationMessageLog_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "Store",
                        principalTable: "IntegrationMessageLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationMessageStatusLog_MessageId",
                schema: "Store",
                table: "IntegrationMessageStatusLog",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebAppRequestContextLogs");

            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "IntegrationMessageStatusLog",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "LoggedApplicationAction",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "LoggedUserInterfaceAction",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "RobotTask",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "IntegrationMessageLog",
                schema: "Store");
        }
    }
}
