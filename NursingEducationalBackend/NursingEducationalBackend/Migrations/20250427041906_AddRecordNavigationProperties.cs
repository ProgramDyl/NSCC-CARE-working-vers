using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingEducationalBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddRecordNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_ADLs_AdlsId1",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_SkinAndSensoryAids_SkinAndSensoryAidsId",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_AdlsId1",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_SkinAndSensoryAidsId",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "AdlsId1",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "SkinAndSensoryAidsId",
                table: "Record");

            migrationBuilder.CreateIndex(
                name: "IX_Record_ADLsID",
                table: "Record",
                column: "ADLsID");

            migrationBuilder.CreateIndex(
                name: "IX_Record_SkinID",
                table: "Record",
                column: "SkinID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_ADLs_ADLsID",
                table: "Record",
                column: "ADLsID",
                principalTable: "ADLs",
                principalColumn: "ADLsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_SkinAndSensoryAids_SkinID",
                table: "Record",
                column: "SkinID",
                principalTable: "SkinAndSensoryAids",
                principalColumn: "SkinAndSensoryAidsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_ADLs_ADLsID",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_SkinAndSensoryAids_SkinID",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_ADLsID",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_SkinID",
                table: "Record");

            migrationBuilder.AddColumn<int>(
                name: "AdlsId1",
                table: "Record",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkinAndSensoryAidsId",
                table: "Record",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Record_AdlsId1",
                table: "Record",
                column: "AdlsId1");

            migrationBuilder.CreateIndex(
                name: "IX_Record_SkinAndSensoryAidsId",
                table: "Record",
                column: "SkinAndSensoryAidsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_ADLs_AdlsId1",
                table: "Record",
                column: "AdlsId1",
                principalTable: "ADLs",
                principalColumn: "ADLsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_SkinAndSensoryAids_SkinAndSensoryAidsId",
                table: "Record",
                column: "SkinAndSensoryAidsId",
                principalTable: "SkinAndSensoryAids",
                principalColumn: "SkinAndSensoryAidsID");
        }
    }
}
