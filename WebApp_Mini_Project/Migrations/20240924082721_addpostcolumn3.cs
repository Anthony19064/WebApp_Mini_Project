using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_Mini_Project.Migrations
{
    /// <inheritdoc />
    public partial class addpostcolumn3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "Posts",
                newName: "timeout");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "timeend",
                table: "Posts",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "timeCreate",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timeCreate",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "timeout",
                table: "Posts",
                newName: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "timeend",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }
    }
}
