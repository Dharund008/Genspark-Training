using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authoratative",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Authoratative",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
