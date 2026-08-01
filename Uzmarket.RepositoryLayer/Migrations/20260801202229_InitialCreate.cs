using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UzMarket.RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "text", nullable: true),
                    Xml = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
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
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
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
                name: "SYS_USER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    PINFL = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    PHONE_NUMBER = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ADDRESS = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DATE_OF_BIRTH = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PASSPORT_SERIES = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    EMAIL = table.Column<string>(type: "text", nullable: false),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StatusIdConst = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_ADDRESS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    REGION = table.Column<string>(type: "text", nullable: false),
                    CITY = table.Column<string>(type: "text", nullable: false),
                    STREET = table.Column<string>(type: "text", nullable: false),
                    POSTAL_CODE = table.Column<string>(type: "text", nullable: false),
                    IS_DIFAULT = table.Column<bool>(type: "boolean", nullable: false),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
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
                name: "SYS_CART",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
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
                name: "SYS_PRODUCT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "text", nullable: true),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    STOCK_QUANTITY = table.Column<int>(type: "integer", nullable: false),
                    CATEGORY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SUPPLIER_ID = table.Column<long>(type: "bigint", nullable: false),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
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
                        name: "FK_SYS_PRODUCT_SYS_USER_SUPPLIER_ID",
                        column: x => x.SUPPLIER_ID,
                        principalTable: "SYS_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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
                    ORDER_STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    SHIPPING_ADDRESS_ID = table.Column<int>(type: "integer", nullable: false),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OrderStatus = table.Column<int>(type: "integer", nullable: false),
                    StatusConst = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ORDER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_ORDER_SYS_ADDRESS_SHIPPING_ADDRESS_ID",
                        column: x => x.SHIPPING_ADDRESS_ID,
                        principalTable: "SYS_ADDRESS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_ORDER_SYS_USER_USER_ID",
                        column: x => x.USER_ID,
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
                name: "SYS_FAVORITE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false)
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
                name: "SYS_PRODUCT_IMAGE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IMAGE_URL = table.Column<string>(type: "text", nullable: false),
                    MAIN_PIC = table.Column<bool>(type: "boolean", nullable: false),
                    SORT_ORDER = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PRODUCT_IMAGE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_PRODUCT_IMAGE_SYS_PRODUCT_ProductId",
                        column: x => x.ProductId,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SYS_REVIEW",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    RATING = table.Column<int>(type: "integer", nullable: true),
                    COMMENT = table.Column<string>(type: "text", nullable: true),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_REVIEW", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_REVIEW_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_REVIEW_SYS_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "SYS_USER",
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

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ADDRESS_USER_ID",
                table: "SYS_ADDRESS",
                column: "USER_ID");

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
                name: "IX_SYS_FAVORITE_PRODUCT_ID",
                table: "SYS_FAVORITE",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_FAVORITE_USER_ID",
                table: "SYS_FAVORITE",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ORDER_SHIPPING_ADDRESS_ID",
                table: "SYS_ORDER",
                column: "SHIPPING_ADDRESS_ID");

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
                name: "IX_SYS_PRODUCT_SUPPLIER_ID",
                table: "SYS_PRODUCT",
                column: "SUPPLIER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_PRODUCT_IMAGE_ProductId",
                table: "SYS_PRODUCT_IMAGE",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_REVIEW_PRODUCT_ID",
                table: "SYS_REVIEW",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_REVIEW_USER_ID",
                table: "SYS_REVIEW",
                column: "USER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "SYS_CART_ITEM");

            migrationBuilder.DropTable(
                name: "SYS_FAVORITE");

            migrationBuilder.DropTable(
                name: "SYS_ORDER_ITEM");

            migrationBuilder.DropTable(
                name: "SYS_PAYMENT");

            migrationBuilder.DropTable(
                name: "SYS_PRODUCT_IMAGE");

            migrationBuilder.DropTable(
                name: "SYS_REVIEW");

            migrationBuilder.DropTable(
                name: "SYS_CART");

            migrationBuilder.DropTable(
                name: "SYS_ORDER");

            migrationBuilder.DropTable(
                name: "SYS_PRODUCT");

            migrationBuilder.DropTable(
                name: "SYS_ADDRESS");

            migrationBuilder.DropTable(
                name: "SYS_CATEGORY");

            migrationBuilder.DropTable(
                name: "SYS_USER");
        }
    }
}
