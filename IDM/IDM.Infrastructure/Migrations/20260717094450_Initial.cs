using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IDM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActual = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ShortName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SourceName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    ParentGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsParentDepartmentDefinedManually = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsHeadOfBranch = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsShowOnScheme = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentGuid",
                        column: x => x.ParentGuid,
                        principalTable: "Departments",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CssClass = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    EmployeeStatusRules = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Normalizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Pattern = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Replacement = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Normalizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    IsActual = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PersonGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Persons_PersonGuid",
                        column: x => x.PersonGuid,
                        principalTable: "Persons",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DepartmentGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PositionGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsActual = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EmploymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DismissalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextPlannedVacationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VacationRemainingDays = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentGuid",
                        column: x => x.DepartmentGuid,
                        principalTable: "Departments",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Persons_PersonGuid",
                        column: x => x.PersonGuid,
                        principalTable: "Persons",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Positions_PositionGuid",
                        column: x => x.PositionGuid,
                        principalTable: "Positions",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Absences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalGuid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absences_Employees_EmployeeGuid",
                        column: x => x.EmployeeGuid,
                        principalTable: "Employees",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    DepartmentGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => new { x.DepartmentGuid, x.EmployeeGuid });
                    table.ForeignKey(
                        name: "FK_Supervisors_Departments_DepartmentGuid",
                        column: x => x.DepartmentGuid,
                        principalTable: "Departments",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Supervisors_Employees_EmployeeGuid",
                        column: x => x.EmployeeGuid,
                        principalTable: "Employees",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absences_EmployeeGuid",
                table: "Absences",
                column: "EmployeeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PersonGuid",
                table: "Contacts",
                column: "PersonGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentGuid",
                table: "Departments",
                column: "ParentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentGuid",
                table: "Employees",
                column: "DepartmentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonGuid",
                table: "Employees",
                column: "PersonGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionGuid",
                table: "Employees",
                column: "PositionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_DepartmentGuid",
                table: "Supervisors",
                column: "DepartmentGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_EmployeeGuid",
                table: "Supervisors",
                column: "EmployeeGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absences");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "EmployeeStatuses");

            migrationBuilder.DropTable(
                name: "Normalizations");

            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
