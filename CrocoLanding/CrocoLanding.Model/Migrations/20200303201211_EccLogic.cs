using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrocoLanding.Model.Migrations
{
    public partial class EccLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ecc");

            migrationBuilder.CreateTable(
                name: "EccChat",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDialog = table.Column<bool>(nullable: false),
                    ChatName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EccChat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EccFile",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EccFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EccUser",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EccUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EccUserGroup",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EccUserGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailGroup",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegratedApp",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AppType = table.Column<int>(nullable: false),
                    Uid = table.Column<string>(maxLength: 128, nullable: true),
                    ConfigurationJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegratedApp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EccChatMessage",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    SentOnUtcTicks = table.Column<long>(nullable: false),
                    ChatId = table.Column<int>(nullable: false),
                    SenderUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EccChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EccChatMessage_EccChat_ChatId",
                        column: x => x.ChatId,
                        principalSchema: "Ecc",
                        principalTable: "EccChat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EccChatMessage_EccUser_SenderUserId",
                        column: x => x.SenderUserId,
                        principalSchema: "Ecc",
                        principalTable: "EccUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EccChatUserRelation",
                schema: "Ecc",
                columns: table => new
                {
                    ChatId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    IsChatCreator = table.Column<bool>(nullable: false),
                    LastVisitUtcTicks = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EccChatUserRelation", x => new { x.ChatId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EccChatUserRelation_EccChat_ChatId",
                        column: x => x.ChatId,
                        principalSchema: "Ecc",
                        principalTable: "EccChat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EccChatUserRelation_EccUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Ecc",
                        principalTable: "EccUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Interaction",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Type = table.Column<string>(maxLength: 24, nullable: false),
                    MessageText = table.Column<string>(nullable: true),
                    TitleText = table.Column<string>(nullable: true),
                    MaskItemsJson = table.Column<string>(nullable: true),
                    SendNow = table.Column<bool>(nullable: false),
                    SendOn = table.Column<DateTime>(nullable: true),
                    SentOn = table.Column<DateTime>(nullable: true),
                    DeliveredOn = table.Column<DateTime>(nullable: false),
                    ReadOn = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ReceiverEmail = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ObjectJson = table.Column<string>(nullable: true),
                    NotificationType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interaction_EccUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Ecc",
                        principalTable: "EccUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EccUserInUserGroupRelation",
                schema: "Ecc",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    UserGroupId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EccUserInUserGroupRelation", x => new { x.UserId, x.UserGroupId });
                    table.ForeignKey(
                        name: "FK_EccUserInUserGroupRelation_EccUserGroup_UserGroupId",
                        column: x => x.UserGroupId,
                        principalSchema: "Ecc",
                        principalTable: "EccUserGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EccUserInUserGroupRelation_EccUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Ecc",
                        principalTable: "EccUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailInEmailGroupRelation",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EmailGroupId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailInEmailGroupRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailInEmailGroupRelation_EmailGroup_EmailGroupId",
                        column: x => x.EmailGroupId,
                        principalSchema: "Ecc",
                        principalTable: "EmailGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EccChatMessageAttachment",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ChatMessageId = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EccChatMessageAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EccChatMessageAttachment_EccChatMessage_ChatMessageId",
                        column: x => x.ChatMessageId,
                        principalSchema: "Ecc",
                        principalTable: "EccChatMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EccChatMessageAttachment_EccFile_FileId",
                        column: x => x.FileId,
                        principalSchema: "Ecc",
                        principalTable: "EccFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InteractionAttachment",
                schema: "Ecc",
                columns: table => new
                {
                    InteractionId = table.Column<string>(nullable: false),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionAttachment", x => new { x.InteractionId, x.FileId });
                    table.ForeignKey(
                        name: "FK_InteractionAttachment_EccFile_FileId",
                        column: x => x.FileId,
                        principalSchema: "Ecc",
                        principalTable: "EccFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionAttachment_Interaction_InteractionId",
                        column: x => x.InteractionId,
                        principalSchema: "Ecc",
                        principalTable: "Interaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InteractionStatusLog",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InteractionId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StartedOn = table.Column<DateTime>(nullable: false),
                    StatusDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionStatusLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InteractionStatusLog_Interaction_InteractionId",
                        column: x => x.InteractionId,
                        principalSchema: "Ecc",
                        principalTable: "Interaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EccChatMessage_ChatId",
                schema: "Ecc",
                table: "EccChatMessage",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_EccChatMessage_SenderUserId",
                schema: "Ecc",
                table: "EccChatMessage",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EccChatMessageAttachment_ChatMessageId",
                schema: "Ecc",
                table: "EccChatMessageAttachment",
                column: "ChatMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_EccChatMessageAttachment_FileId",
                schema: "Ecc",
                table: "EccChatMessageAttachment",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_EccChatUserRelation_UserId",
                schema: "Ecc",
                table: "EccChatUserRelation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EccUserGroup_Name",
                schema: "Ecc",
                table: "EccUserGroup",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EccUserInUserGroupRelation_UserGroupId",
                schema: "Ecc",
                table: "EccUserInUserGroupRelation",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailInEmailGroupRelation_EmailGroupId_Email",
                schema: "Ecc",
                table: "EmailInEmailGroupRelation",
                columns: new[] { "EmailGroupId", "Email" },
                unique: true,
                filter: "[EmailGroupId] IS NOT NULL AND [Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IntegratedApp_Uid",
                schema: "Ecc",
                table: "IntegratedApp",
                column: "Uid",
                unique: true,
                filter: "[Uid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Interaction_Type",
                schema: "Ecc",
                table: "Interaction",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Interaction_UserId",
                schema: "Ecc",
                table: "Interaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionAttachment_FileId",
                schema: "Ecc",
                table: "InteractionAttachment",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionStatusLog_InteractionId",
                schema: "Ecc",
                table: "InteractionStatusLog",
                column: "InteractionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EccChatMessageAttachment",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccChatUserRelation",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccUserInUserGroupRelation",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EmailInEmailGroupRelation",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "IntegratedApp",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "InteractionAttachment",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "InteractionStatusLog",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccChatMessage",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccUserGroup",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EmailGroup",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccFile",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "Interaction",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccChat",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccUser",
                schema: "Ecc");
        }
    }
}
