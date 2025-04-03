using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations.SocialDb
{
    /// <inheritdoc />
    public partial class SocialV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "social");

            migrationBuilder.CreateTable(
                name: "SocialBlog",
                schema: "social",
                columns: table => new
                {
                    blogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blogTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blogContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blogStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blogLike = table.Column<int>(type: "int", nullable: false),
                    blogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialBlog", x => x.blogID);
                });

            migrationBuilder.CreateTable(
                name: "socialGroups",
                columns: table => new
                {
                    groupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    groupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    groupDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    groupStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    srcID = table.Column<int>(type: "int", nullable: false),
                    groupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialGroups", x => x.groupID);
                });

            migrationBuilder.CreateTable(
                name: "SocialSrc",
                schema: "social",
                columns: table => new
                {
                    srcID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    srcName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    srcTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imageSrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    srcDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialSrc", x => x.srcID);
                });

            migrationBuilder.CreateTable(
                name: "socialBlogComments",
                columns: table => new
                {
                    commentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blogID = table.Column<int>(type: "int", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: false),
                    commentContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    commentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    commentLike = table.Column<int>(type: "int", nullable: false),
                    commentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialBlogComments", x => x.commentID);
                    table.ForeignKey(
                        name: "FK_socialBlogComments_SocialBlog_blogID",
                        column: x => x.blogID,
                        principalSchema: "social",
                        principalTable: "SocialBlog",
                        principalColumn: "blogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "socialGroupUsers",
                columns: table => new
                {
                    socialGroupUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    groupID = table.Column<int>(type: "int", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: false),
                    memberDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialGroupUsers", x => x.socialGroupUserID);
                    table.ForeignKey(
                        name: "FK_socialGroupUsers_socialGroups_groupID",
                        column: x => x.groupID,
                        principalTable: "socialGroups",
                        principalColumn: "groupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "socialMessages",
                columns: table => new
                {
                    messageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    senderID = table.Column<int>(type: "int", nullable: false),
                    receiverID = table.Column<int>(type: "int", nullable: false),
                    groupID = table.Column<int>(type: "int", nullable: false),
                    messageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    messageStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    messageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocialGroupgroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialMessages", x => x.messageID);
                    table.ForeignKey(
                        name: "FK_socialMessages_socialGroups_SocialGroupgroupID",
                        column: x => x.SocialGroupgroupID,
                        principalTable: "socialGroups",
                        principalColumn: "groupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "socialSrcBlogs",
                columns: table => new
                {
                    srcBlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    srcID = table.Column<int>(type: "int", nullable: false),
                    blogID = table.Column<int>(type: "int", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false),
                    srcBlogDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialSrcBlogs", x => x.srcBlogID);
                    table.ForeignKey(
                        name: "FK_socialSrcBlogs_SocialBlog_blogID",
                        column: x => x.blogID,
                        principalSchema: "social",
                        principalTable: "SocialBlog",
                        principalColumn: "blogID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_socialSrcBlogs_SocialSrc_srcID",
                        column: x => x.srcID,
                        principalSchema: "social",
                        principalTable: "SocialSrc",
                        principalColumn: "srcID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "socialSrcMessages",
                columns: table => new
                {
                    srcMessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    srcID = table.Column<int>(type: "int", nullable: false),
                    messageID = table.Column<int>(type: "int", nullable: false),
                    srcMessageDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialSrcMessages", x => x.srcMessageID);
                    table.ForeignKey(
                        name: "FK_socialSrcMessages_SocialSrc_srcID",
                        column: x => x.srcID,
                        principalSchema: "social",
                        principalTable: "SocialSrc",
                        principalColumn: "srcID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_socialSrcMessages_socialMessages_messageID",
                        column: x => x.messageID,
                        principalTable: "socialMessages",
                        principalColumn: "messageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_socialBlogComments_blogID",
                table: "socialBlogComments",
                column: "blogID");

            migrationBuilder.CreateIndex(
                name: "IX_socialGroupUsers_groupID",
                table: "socialGroupUsers",
                column: "groupID");

            migrationBuilder.CreateIndex(
                name: "IX_socialMessages_SocialGroupgroupID",
                table: "socialMessages",
                column: "SocialGroupgroupID");

            migrationBuilder.CreateIndex(
                name: "IX_socialSrcBlogs_blogID",
                table: "socialSrcBlogs",
                column: "blogID");

            migrationBuilder.CreateIndex(
                name: "IX_socialSrcBlogs_srcID",
                table: "socialSrcBlogs",
                column: "srcID");

            migrationBuilder.CreateIndex(
                name: "IX_socialSrcMessages_messageID",
                table: "socialSrcMessages",
                column: "messageID");

            migrationBuilder.CreateIndex(
                name: "IX_socialSrcMessages_srcID",
                table: "socialSrcMessages",
                column: "srcID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "socialBlogComments");

            migrationBuilder.DropTable(
                name: "socialGroupUsers");

            migrationBuilder.DropTable(
                name: "socialSrcBlogs");

            migrationBuilder.DropTable(
                name: "socialSrcMessages");

            migrationBuilder.DropTable(
                name: "SocialBlog",
                schema: "social");

            migrationBuilder.DropTable(
                name: "SocialSrc",
                schema: "social");

            migrationBuilder.DropTable(
                name: "socialMessages");

            migrationBuilder.DropTable(
                name: "socialGroups");
        }
    }
}
