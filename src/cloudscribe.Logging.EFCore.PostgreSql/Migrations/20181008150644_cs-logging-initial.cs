using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Logging.EFCore.PostgreSql.Migrations
{
    public partial class cslogginginitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''");

            migrationBuilder.CreateSequence(
                name: "LogIds");

            migrationBuilder.CreateTable(
                name: "cs_system_log",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    log_date = table.Column<DateTime>(nullable: false),
                    ip_address = table.Column<string>(maxLength: 50, nullable: true),
                    culture = table.Column<string>(maxLength: 10, nullable: true),
                    url = table.Column<string>(nullable: true),
                    short_url = table.Column<string>(maxLength: 255, nullable: true),
                    thread = table.Column<string>(maxLength: 255, nullable: true),
                    log_level = table.Column<string>(maxLength: 20, nullable: true),
                    logger = table.Column<string>(maxLength: 255, nullable: true),
                    message = table.Column<string>(nullable: true),
                    state_json = table.Column<string>(nullable: true),
                    event_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_system_log", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_system_log");

            migrationBuilder.DropSequence(
                name: "LogIds");
        }
    }
}
