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

            migrationBuilder.EnsureSchema(
                name: "Clt");

            migrationBuilder.EnsureSchema(
                name: "Ecc");

            migrationBuilder.CreateTable(
                name: "CallBackRequests",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmailOrPhoneNumber = table.Column<string>(maxLength: 64, nullable: true),
                    IpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    IsNotified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallBackRequests", x => x.Id);
                });

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
                name: "AspNetRoles",
                schema: "Clt",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "Clt",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                name: "EmailTemplate",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomEmailType = table.Column<string>(nullable: true),
                    JsScript = table.Column<string>(nullable: true),
                    IsJsScripted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Clt",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Clt",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Clt",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Clt",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "Clt",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Clt",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Clt",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IntegratedAppUserSetting",
                schema: "Ecc",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    AppId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    UserUidInApp = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegratedAppUserSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegratedAppUserSetting_IntegratedApp_AppId",
                        column: x => x.AppId,
                        principalSchema: "Ecc",
                        principalTable: "IntegratedApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntegratedAppUserSetting_EccUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Ecc",
                        principalTable: "EccUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationMessageStatusLog",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HandlerId = table.Column<string>(maxLength: 128, nullable: true),
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Clt",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Clt",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Clt",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Clt",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "IX_IntegratedAppUserSetting_AppId",
                schema: "Ecc",
                table: "IntegratedAppUserSetting",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegratedAppUserSetting_UserId",
                schema: "Ecc",
                table: "IntegratedAppUserSetting",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationMessageStatusLog_MessageId",
                schema: "Store",
                table: "IntegrationMessageStatusLog",
                column: "MessageId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CallBackRequests");

            migrationBuilder.DropTable(
                name: "WebAppRequestContextLogs");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "Clt");

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
                name: "EmailTemplate",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "IntegratedAppUserSetting",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "InteractionAttachment",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "InteractionStatusLog",
                schema: "Ecc");

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
                name: "AspNetRoles",
                schema: "Clt");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "Clt");

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
                name: "IntegratedApp",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccFile",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "Interaction",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "IntegrationMessageLog",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "EccChat",
                schema: "Ecc");

            migrationBuilder.DropTable(
                name: "EccUser",
                schema: "Ecc");
        }
    }
}
