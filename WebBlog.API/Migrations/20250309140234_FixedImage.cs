using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBlog.API.Migrations
{
    /// <inheritdoc />
    public partial class FixedImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Iamge",
                table: "Blogs",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Blogs",
                newName: "Iamge");
        }
    }
}
