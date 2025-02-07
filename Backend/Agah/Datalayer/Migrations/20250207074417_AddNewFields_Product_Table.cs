using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFields_Product_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Product",
                newName: "PersianName");

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                table: "Product",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IconName",
                table: "Product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishName",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IconName",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "PersianName",
                table: "Product",
                newName: "Title");
        }
    }
}
