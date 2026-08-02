using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UzMarket.RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class dddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IS_DIFAULT",
                table: "SYS_ADDRESS",
                newName: "IS_DEFAULT");

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "SYS_CATEGORY",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "SYS_CATEGORY",
                type: "character varying(700)",
                maxLength: 700,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STREET",
                table: "SYS_ADDRESS",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "REGION",
                table: "SYS_ADDRESS",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "POSTAL_CODE",
                table: "SYS_ADDRESS",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CITY",
                table: "SYS_ADDRESS",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IS_DEFAULT",
                table: "SYS_ADDRESS",
                newName: "IS_DIFAULT");

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "SYS_CATEGORY",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "SYS_CATEGORY",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(700)",
                oldMaxLength: 700,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STREET",
                table: "SYS_ADDRESS",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "REGION",
                table: "SYS_ADDRESS",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "POSTAL_CODE",
                table: "SYS_ADDRESS",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CITY",
                table: "SYS_ADDRESS",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
