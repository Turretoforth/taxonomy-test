using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxonomy.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Height = table.Column<uint>(type: "INTEGER", nullable: false),
                    ManagerIdentifier = table.Column<Guid>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    ProgrammingLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    DepartmentName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ManagerIdentifier",
                        column: x => x.ManagerIdentifier,
                        principalTable: "Employees",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerIdentifier",
                table: "Employees",
                column: "ManagerIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
