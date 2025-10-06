using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bts.Migrations
{
    /// <inheritdoc />
    public partial class bugtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Bugs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Bugs");
        }
    }
}
