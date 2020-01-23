using Microsoft.EntityFrameworkCore.Migrations;

namespace CrocoLanding.Model.Migrations
{
    public partial class NewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HandlerId",
                schema: "Store",
                table: "IntegrationMessageStatusLog",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WebAppRequestContextLogs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Uri",
                table: "WebAppRequestContextLogs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestId",
                table: "WebAppRequestContextLogs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotified",
                table: "CallBackRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_LoggedApplicationAction_EventId",
                schema: "Store",
                table: "LoggedApplicationAction",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_LoggedApplicationAction_TransactionId",
                schema: "Store",
                table: "LoggedApplicationAction",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_WebAppRequestContextLogs_RequestId",
                table: "WebAppRequestContextLogs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WebAppRequestContextLogs_StartedOn",
                table: "WebAppRequestContextLogs",
                column: "StartedOn");

            migrationBuilder.CreateIndex(
                name: "IX_WebAppRequestContextLogs_Uri",
                table: "WebAppRequestContextLogs",
                column: "Uri");

            migrationBuilder.CreateIndex(
                name: "IX_WebAppRequestContextLogs_UserId",
                table: "WebAppRequestContextLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LoggedApplicationAction_EventId",
                schema: "Store",
                table: "LoggedApplicationAction");

            migrationBuilder.DropIndex(
                name: "IX_LoggedApplicationAction_TransactionId",
                schema: "Store",
                table: "LoggedApplicationAction");

            migrationBuilder.DropIndex(
                name: "IX_WebAppRequestContextLogs_RequestId",
                table: "WebAppRequestContextLogs");

            migrationBuilder.DropIndex(
                name: "IX_WebAppRequestContextLogs_StartedOn",
                table: "WebAppRequestContextLogs");

            migrationBuilder.DropIndex(
                name: "IX_WebAppRequestContextLogs_Uri",
                table: "WebAppRequestContextLogs");

            migrationBuilder.DropIndex(
                name: "IX_WebAppRequestContextLogs_UserId",
                table: "WebAppRequestContextLogs");

            migrationBuilder.DropColumn(
                name: "HandlerId",
                schema: "Store",
                table: "IntegrationMessageStatusLog");

            migrationBuilder.DropColumn(
                name: "IsNotified",
                table: "CallBackRequests");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WebAppRequestContextLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Uri",
                table: "WebAppRequestContextLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestId",
                table: "WebAppRequestContextLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
