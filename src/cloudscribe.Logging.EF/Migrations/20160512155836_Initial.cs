using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace cloudscribe.Logging.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "LogIds");
            migrationBuilder.CreateTable(
                name: "cs_SystemLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Culture = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    LogDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    LogLevel = table.Column<string>(nullable: true),
                    Logger = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    ShortUrl = table.Column<string>(nullable: true),
                    Thread = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogItem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence("LogIds");
            migrationBuilder.DropTable("cs_SystemLog");
        }
    }
}
