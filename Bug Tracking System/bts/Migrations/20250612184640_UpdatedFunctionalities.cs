using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bts.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFunctionalities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_Developers_AssignedTo",
                table: "Bugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_Testers_CreatedBy",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Testers");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Admins");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Testers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Developers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_Developers_AssignedTo",
                table: "Bugs",
                column: "AssignedTo",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_Testers_CreatedBy",
                table: "Bugs",
                column: "CreatedBy",
                principalTable: "Testers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_Developers_AssignedTo",
                table: "Bugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_Testers_CreatedBy",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Testers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Developers");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Testers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Developers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Admins",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_Developers_AssignedTo",
                table: "Bugs",
                column: "AssignedTo",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_Testers_CreatedBy",
                table: "Bugs",
                column: "CreatedBy",
                principalTable: "Testers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
