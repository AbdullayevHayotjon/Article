using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Article.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class secondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "create_date",
                schema: "HelpSchema",
                table: "TempUsers");

            migrationBuilder.DropColumn(
                name: "delete_date",
                schema: "HelpSchema",
                table: "TempUsers");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "HelpSchema",
                table: "TempUsers");

            migrationBuilder.DropColumn(
                name: "update_date",
                schema: "HelpSchema",
                table: "TempUsers");

            migrationBuilder.RenameColumn(
                name: "last_name",
                schema: "MainSchema",
                table: "Users",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "first_name",
                schema: "MainSchema",
                table: "Users",
                newName: "firstname");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "HelpSchema",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "HelpSchema");

            migrationBuilder.RenameColumn(
                name: "lastname",
                schema: "MainSchema",
                table: "Users",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "firstname",
                schema: "MainSchema",
                table: "Users",
                newName: "first_name");

            migrationBuilder.AddColumn<DateTime>(
                name: "create_date",
                schema: "HelpSchema",
                table: "TempUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_date",
                schema: "HelpSchema",
                table: "TempUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "HelpSchema",
                table: "TempUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "update_date",
                schema: "HelpSchema",
                table: "TempUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
