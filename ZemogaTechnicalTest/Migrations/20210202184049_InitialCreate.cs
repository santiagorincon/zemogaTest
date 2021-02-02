using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZemogaTechnicalTest.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleCode = table.Column<string>(nullable: false),
                    RoleName = table.Column<string>(nullable: false),
                    RoleDesc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusCode = table.Column<string>(nullable: false),
                    StatusName = table.Column<string>(nullable: false),
                    StatusDesc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserFullname = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PostName = table.Column<string>(nullable: false),
                    PostContent = table.Column<string>(nullable: false),
                    AuthorID = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    ApprovalDate = table.Column<DateTime>(nullable: true),
                    EditorID = table.Column<int>(nullable: true),
                    StatusID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Post_User_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_User_EditorID",
                        column: x => x.EditorID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_Status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostActivity",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PostID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    OldStatusID = table.Column<int>(nullable: true),
                    NewStatusID = table.Column<int>(nullable: true),
                    ActivityDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostActivity", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PostActivity_Status_NewStatusID",
                        column: x => x.NewStatusID,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostActivity_Status_OldStatusID",
                        column: x => x.OldStatusID,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostActivity_Post_PostID",
                        column: x => x.PostID,
                        principalTable: "Post",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostActivity_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: true),
                    PostID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PostComments_Post_PostID",
                        column: x => x.PostID,
                        principalTable: "Post",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostComments_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_AuthorID",
                table: "Post",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_Post_EditorID",
                table: "Post",
                column: "EditorID");

            migrationBuilder.CreateIndex(
                name: "IX_Post_StatusID",
                table: "Post",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PostActivity_NewStatusID",
                table: "PostActivity",
                column: "NewStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PostActivity_OldStatusID",
                table: "PostActivity",
                column: "OldStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PostActivity_PostID",
                table: "PostActivity",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_PostActivity_UserID",
                table: "PostActivity",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_PostID",
                table: "PostComments",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_UserID",
                table: "PostComments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostActivity");

            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
