using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UzMarket.RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class rrr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PASSWORD_HASH",
                table: "SYS_PENDING_REGISTRATIONS",
                newName: "USER_NAME");

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS",
                table: "SYS_PENDING_REGISTRATIONS",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DATE_OF_BIRTH",
                table: "SYS_PENDING_REGISTRATIONS",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PASSPORT_SERIES",
                table: "SYS_PENDING_REGISTRATIONS",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PASSWORD",
                table: "SYS_PENDING_REGISTRATIONS",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PHONE_NUMBER",
                table: "SYS_PENDING_REGISTRATIONS",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PINFL",
                table: "SYS_PENDING_REGISTRATIONS",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SHORT_NAME",
                table: "SYS_PENDING_REGISTRATIONS",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ADDRESS",
                table: "SYS_PENDING_REGISTRATIONS");

            migrationBuilder.DropColumn(
                name: "DATE_OF_BIRTH",
                table: "SYS_PENDING_REGISTRATIONS");

            migrationBuilder.DropColumn(
                name: "PASSPORT_SERIES",
                table: "SYS_PENDING_REGISTRATIONS");

            migrationBuilder.DropColumn(
                name: "PASSWORD",
                table: "SYS_PENDING_REGISTRATIONS");

            migrationBuilder.DropColumn(
                name: "PHONE_NUMBER",
                table: "SYS_PENDING_REGISTRATIONS");

            migrationBuilder.DropColumn(
                name: "PINFL",
                table: "SYS_PENDING_REGISTRATIONS");

            migrationBuilder.DropColumn(
                name: "SHORT_NAME",
                table: "SYS_PENDING_REGISTRATIONS");

            migrationBuilder.RenameColumn(
                name: "USER_NAME",
                table: "SYS_PENDING_REGISTRATIONS",
                newName: "PASSWORD_HASH");
        }
    }
}
