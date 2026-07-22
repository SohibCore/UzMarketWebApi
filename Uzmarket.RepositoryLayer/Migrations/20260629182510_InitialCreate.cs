using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata; 
using Microsoft.EntityFrameworkCore.Migrations;

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
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_USER");
        }
    }
}
