using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "BlogNews",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        Content = table.Column<string>(type: "text", nullable: false),
            //        Time = table.Column<DateTime>(type: "datetime", nullable: false),
            //        BrowseCount = table.Column<int>(type: "int", nullable: false),
            //        LikeCount = table.Column<int>(type: "int", nullable: false),
            //        TypeId = table.Column<int>(type: "int", nullable: false),
            //        WriterId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BlogNews_Id", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TypeInfo",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TypeInfo_Id", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "WriterInfo",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
            //        UserName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
            //        UserPwd = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WriterInfo_Id", x => x.Id);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BlogNews");

            //migrationBuilder.DropTable(
            //    name: "TypeInfo");

            //migrationBuilder.DropTable(
            //    name: "WriterInfo");
        }
    }
}
