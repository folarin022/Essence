using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EssenceShop.Migrations
{
    /// <inheritdoc />
    public partial class AnotherColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Available",
                table: "Clothes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "null");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "Clothes");
        }
    }
}
