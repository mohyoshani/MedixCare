using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedixCare.Migrations
{
    /// <inheritdoc />
    public partial class addingEnumTestType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestType",
                table: "LabTests",
                newName: "testType");

            migrationBuilder.AlterColumn<int>(
                name: "testType",
                table: "LabTests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "testType",
                table: "LabTests",
                newName: "TestType");

            migrationBuilder.AlterColumn<string>(
                name: "TestType",
                table: "LabTests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
