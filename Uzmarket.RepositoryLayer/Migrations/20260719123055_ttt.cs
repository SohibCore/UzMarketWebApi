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
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_PRODUCT_SYS_USER_UserId",
                table: "SYS_PRODUCT");

            migrationBuilder.DropIndex(
                name: "IX_SYS_PRODUCT_UserId",
                table: "SYS_PRODUCT");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SYS_PRODUCT");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_PRODUCT_SUPPLIER_ID",
                table: "SYS_PRODUCT",
                column: "SUPPLIER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_PRODUCT_SYS_USER_SUPPLIER_ID",
                table: "SYS_PRODUCT",
                column: "SUPPLIER_ID",
                principalTable: "SYS_USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_PRODUCT_SYS_USER_SUPPLIER_ID",
                table: "SYS_PRODUCT");

            migrationBuilder.DropIndex(
                name: "IX_SYS_PRODUCT_SUPPLIER_ID",
                table: "SYS_PRODUCT");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "SYS_PRODUCT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SYS_PRODUCT_UserId",
                table: "SYS_PRODUCT",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_PRODUCT_SYS_USER_UserId",
                table: "SYS_PRODUCT",
                column: "UserId",
                principalTable: "SYS_USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
