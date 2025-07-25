using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicForm.Common.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20250714004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "RoleMenuPermission",
                newName: "CreateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "RoleMenuPermission",
                newName: "Created");
        }
    }
}
