using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UzMarket.RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_PENDING_REGISTRATIONS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EMAIL = table.Column<string>(type: "text", nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "text", nullable: false),
                    FULL_NAME = table.Column<string>(type: "text", nullable: false),
                    CODE = table.Column<string>(type: "text", nullable: false),
                    EXPIRES_AT = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ATTEMPT_COUNT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PENDING_REGISTRATIONS", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_PENDING_REGISTRATIONS");
        }
    }
}
