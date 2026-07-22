using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UzMarket.RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class BuildAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ORDER_DATE",
                table: "SYS_ORDER",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<long>(
                name: "SHIPPING_ADDRESS_ID",
                table: "SYS_ORDER",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "SYS_ADDRESS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    REGION = table.Column<string>(type: "text", nullable: false),
                    CITY = table.Column<string>(type: "text", nullable: false),
                    STREET = table.Column<string>(type: "text", nullable: false),
                    POSTAL_CODE = table.Column<string>(type: "text", nullable: false),
                    IS_DIFAULT = table.Column<bool>(type: "boolean", nullable: false),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ADDRESS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_ADDRESS_SYS_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "SYS_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_FAVORITE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_FAVORITE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_FAVORITE_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_FAVORITE_SYS_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "SYS_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_PAYMENT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ORDER_ID = table.Column<long>(type: "bigint", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "numeric", nullable: false),
                    PAYMENT_METHOD_ID = table.Column<int>(type: "integer", nullable: false),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    TRANSACTION_DATE = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    OrderStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PAYMENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_REVIEW",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    RATING = table.Column<int>(type: "integer", nullable: false),
                    COMMENT = table.Column<string>(type: "text", nullable: true),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_REVIEW", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ORDER_SHIPPING_ADDRESS_ID",
                table: "SYS_ORDER",
                column: "SHIPPING_ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ADDRESS_USER_ID",
                table: "SYS_ADDRESS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_FAVORITE_PRODUCT_ID",
                table: "SYS_FAVORITE",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_FAVORITE_USER_ID",
                table: "SYS_FAVORITE",
                column: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_ORDER_SYS_ADDRESS_SHIPPING_ADDRESS_ID",
                table: "SYS_ORDER",
                column: "SHIPPING_ADDRESS_ID",
                principalTable: "SYS_ADDRESS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_ORDER_SYS_ADDRESS_SHIPPING_ADDRESS_ID",
                table: "SYS_ORDER");

            migrationBuilder.DropTable(
                name: "SYS_ADDRESS");

            migrationBuilder.DropTable(
                name: "SYS_FAVORITE");

            migrationBuilder.DropTable(
                name: "SYS_PAYMENT");

            migrationBuilder.DropTable(
                name: "SYS_REVIEW");

            migrationBuilder.DropIndex(
                name: "IX_SYS_ORDER_SHIPPING_ADDRESS_ID",
                table: "SYS_ORDER");

            migrationBuilder.DropColumn(
                name: "SHIPPING_ADDRESS_ID",
                table: "SYS_ORDER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ORDER_DATE",
                table: "SYS_ORDER",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
