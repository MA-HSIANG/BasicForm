using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicForm.Common.Migrations
{
    /// <inheritdoc />
    public partial class _20250725001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "RoleMenuPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoleMenuPermission");
        }
    }
}
