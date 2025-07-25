using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicForm.Common.Migrations
{
    /// <inheritdoc />
    public partial class _20250717001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreateId",
                table: "UserLoginInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "UserLoginInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "UserLoginInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "UserLoginInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdateId",
                table: "UserLoginInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "UserLoginInfo",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateId",
                table: "UserLoginInfo");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "UserLoginInfo");

            migrationBuilder.DropColumn(
                name: "Enable",
                table: "UserLoginInfo");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserLoginInfo");

            migrationBuilder.DropColumn(
                name: "UpdateId",
                table: "UserLoginInfo");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "UserLoginInfo");
        }
    }
}
