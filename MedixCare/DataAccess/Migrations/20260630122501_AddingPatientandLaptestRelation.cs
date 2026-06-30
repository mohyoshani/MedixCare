using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedixCare.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingPatientandLaptestRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId1",
                table: "LabTests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabTests_PatientId1",
                table: "LabTests",
                column: "PatientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_Patients_PatientId1",
                table: "LabTests",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_Patients_PatientId1",
                table: "LabTests");

            migrationBuilder.DropIndex(
                name: "IX_LabTests_PatientId1",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "LabTests");
        }
    }
}
