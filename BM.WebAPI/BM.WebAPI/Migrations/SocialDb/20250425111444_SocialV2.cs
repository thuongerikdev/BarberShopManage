using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations.SocialDb
{
    /// <inheritdoc />
    public partial class SocialV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "socialSrcBlogs");

            migrationBuilder.DropTable(
                name: "socialSrcMessages");

            migrationBuilder.AddColumn<string>(
                name: "blogDescription",
                schema: "social",
                table: "SocialBlog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "blogImage",
                schema: "social",
                table: "SocialBlog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "socialBlogContents",
                columns: table => new
                {
                    contentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blogID = table.Column<int>(type: "int", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false),
                    contentTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialBlogContents", x => x.contentID);
                    table.ForeignKey(
                        name: "FK_socialBlogContents_SocialBlog_blogID",
                        column: x => x.blogID,
                        principalSchema: "social",
                        principalTable: "SocialBlog",
                        principalColumn: "blogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "socialBlogImages",
                columns: table => new
                {
                    blogImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blogID = table.Column<int>(type: "int", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false),
                    srcImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    topicImageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialBlogImages", x => x.blogImageID);
                    table.ForeignKey(
                        name: "FK_socialBlogImages_SocialBlog_blogID",
                        column: x => x.blogID,
                        principalSchema: "social",
                        principalTable: "SocialBlog",
                        principalColumn: "blogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "socialBlogTopics",
                columns: table => new
                {
                    topicID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blogID = table.Column<int>(type: "int", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false),
                    topicTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    topicContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    topicStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    topicDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialBlogTopics", x => x.topicID);
                    table.ForeignKey(
                        name: "FK_socialBlogTopics_SocialBlog_blogID",
                        column: x => x.blogID,
                        principalSchema: "social",
                        principalTable: "SocialBlog",
                        principalColumn: "blogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_socialBlogContents_blogID",
                table: "socialBlogContents",
                column: "blogID");

            migrationBuilder.CreateIndex(
                name: "IX_socialBlogImages_blogID",
                table: "socialBlogImages",
                column: "blogID");

            migrationBuilder.CreateIndex(
                name: "IX_socialBlogTopics_blogID",
                table: "socialBlogTopics",
                column: "blogID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "socialBlogContents");

            migrationBuilder.DropTable(
                name: "socialBlogImages");

            migrationBuilder.DropTable(
                name: "socialBlogTopics");

            migrationBuilder.DropColumn(
                name: "blogDescription",
                schema: "social",
                table: "SocialBlog");

            migrationBuilder.DropColumn(
                name: "blogImage",
                schema: "social",
                table: "SocialBlog");

            migrationBuilder.CreateTable(
                name: "socialSrcBlogs",
                columns: table => new
                {
                    srcBlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blogID = table.Column<int>(type: "int", nullable: false),
                    srcID = table.Column<int>(type: "int", nullable: false),
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
                    messageID = table.Column<int>(type: "int", nullable: false),
                    srcID = table.Column<int>(type: "int", nullable: false),
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
    }
}
