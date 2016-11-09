using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Logging.EFCore.MSSQL.Migrations
{
    public partial class changes20160701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "cs_SystemLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StateJson",
                table: "cs_SystemLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "cs_SystemLog");

            migrationBuilder.DropColumn(
                name: "StateJson",
                table: "cs_SystemLog");
        }
    }
}
