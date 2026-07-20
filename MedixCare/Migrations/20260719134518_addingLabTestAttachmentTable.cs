using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedixCare.Migrations
{
    /// <inheritdoc />
    public partial class addingLabTestAttachmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestFileName",
                table: "LabTests");

            migrationBuilder.CreateTable(
                name: "LabTestsAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LabTestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestsAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabTestsAttachment_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabTestsAttachment_LabTestId",
                table: "LabTestsAttachment",
                column: "LabTestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabTestsAttachment");

            migrationBuilder.AddColumn<string>(
                name: "TestFileName",
                table: "LabTests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
