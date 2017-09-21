using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Reminder.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletionDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    LastUpdateDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminders");
        }
    }
}
