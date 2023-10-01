using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oracle.Database.Migrations
{
    /// <inheritdoc />
    public partial class indexrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_user_departmentId",
                table: "user",
                newName: "IX_user_departmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_user_departmentId",
                table: "user",
                newName: "ix_user_departmentId");
        }
    }
}
