using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UzMarket.RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class ttt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_EMAIL_CONFIRMED",
                table: "SYS_USER",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_EMAIL_CONFIRMED",
                table: "SYS_USER");
        }
    }
}
