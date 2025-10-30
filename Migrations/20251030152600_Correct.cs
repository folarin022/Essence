using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EssenceShop.Migrations
{
    /// <inheritdoc />
    public partial class Correct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Available",
                table: "Clothes",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "CollectedDate",
                table: "Clothes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCollected",
                table: "Clothes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectedDate",
                table: "Clothes");

            migrationBuilder.DropColumn(
                name: "IsCollected",
                table: "Clothes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clothes",
                newName: "Available");
        }
    }
}
