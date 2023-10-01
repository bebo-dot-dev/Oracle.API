using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oracle.Database.Migrations
{
    /// <inheritdoc />
    public partial class userrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    employeeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    userName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    firstName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    lastName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    departmentId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.employeeId);
                    table.ForeignKey(
                        name: "FK_employee_department_departmentId",
                        column: x => x.departmentId,
                        principalTable: "department",
                        principalColumn: "departmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee_departmentId",
                table: "employee",
                column: "departmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    userId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    departmentId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    firstName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    lastName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    userName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.userId);
                    table.ForeignKey(
                        name: "FK_user_department_departmentId",
                        column: x => x.departmentId,
                        principalTable: "department",
                        principalColumn: "departmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_departmentId",
                table: "user",
                column: "departmentId");
        }
    }
}
