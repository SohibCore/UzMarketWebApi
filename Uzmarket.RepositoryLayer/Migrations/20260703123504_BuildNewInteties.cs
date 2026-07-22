using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UzMarket.RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class BuildNewInteties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_CART",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_CART", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_CART_SYS_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "SYS_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_CATEGORY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "text", nullable: true),
                    PARENT_CATEGORY_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_CATEGORY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_CATEGORY_SYS_CATEGORY_PARENT_CATEGORY_ID",
                        column: x => x.PARENT_CATEGORY_ID,
                        principalTable: "SYS_CATEGORY",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SYS_ORDER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    ORDER_DATE = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TOTAL_AMOUNT = table.Column<decimal>(type: "numeric", nullable: false),
                    STATUS_ID = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ORDER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_ORDER_SYS_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "SYS_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_PRODUCT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "text", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    STOCK_QUANTITY = table.Column<int>(type: "integer", nullable: false),
                    CATEGORY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SUPPLIER_ID = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PRODUCT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_PRODUCT_SYS_CATEGORY_CATEGORY_ID",
                        column: x => x.CATEGORY_ID,
                        principalTable: "SYS_CATEGORY",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_PRODUCT_SYS_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "SYS_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_CART_ITEM",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CART_ID = table.Column<long>(type: "bigint", nullable: false),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false),
                    QUANTITY = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_CART_ITEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_CART_ITEM_SYS_CART_CART_ID",
                        column: x => x.CART_ID,
                        principalTable: "SYS_CART",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_CART_ITEM_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_ORDER_ITEM",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ORDER_ID = table.Column<long>(type: "bigint", nullable: false),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false),
                    QUANTITY = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ORDER_ITEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_ORDER_ITEM_SYS_ORDER_ORDER_ID",
                        column: x => x.ORDER_ID,
                        principalTable: "SYS_ORDER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_ORDER_ITEM_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_PRODUCT_IMAGE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IMAGE_URL = table.Column<string>(type: "text", nullable: false),
                    MAIN_PIC = table.Column<bool>(type: "boolean", nullable: false),
                    SORT_ORDER = table.Column<int>(type: "integer", nullable: false),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PRODUCT_IMAGE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_PRODUCT_IMAGE_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_CART_USER_ID",
                table: "SYS_CART",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_CART_ITEM_CART_ID",
                table: "SYS_CART_ITEM",
                column: "CART_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_CART_ITEM_PRODUCT_ID",
                table: "SYS_CART_ITEM",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_CATEGORY_PARENT_CATEGORY_ID",
                table: "SYS_CATEGORY",
                column: "PARENT_CATEGORY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ORDER_USER_ID",
                table: "SYS_ORDER",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ORDER_ITEM_ORDER_ID",
                table: "SYS_ORDER_ITEM",
                column: "ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ORDER_ITEM_PRODUCT_ID",
                table: "SYS_ORDER_ITEM",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_PRODUCT_CATEGORY_ID",
                table: "SYS_PRODUCT",
                column: "CATEGORY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_PRODUCT_UserId",
                table: "SYS_PRODUCT",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_PRODUCT_IMAGE_PRODUCT_ID",
                table: "SYS_PRODUCT_IMAGE",
                column: "PRODUCT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_CART_ITEM");

            migrationBuilder.DropTable(
                name: "SYS_ORDER_ITEM");

            migrationBuilder.DropTable(
                name: "SYS_PRODUCT_IMAGE");

            migrationBuilder.DropTable(
                name: "SYS_CART");

            migrationBuilder.DropTable(
                name: "SYS_ORDER");

            migrationBuilder.DropTable(
                name: "SYS_PRODUCT");

            migrationBuilder.DropTable(
                name: "SYS_CATEGORY");
        }
    }
}
