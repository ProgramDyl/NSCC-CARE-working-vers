using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingEducationalBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedRecordSubTableRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_Record_BehaviourID",
                table: "Record",
                column: "BehaviourID");

            migrationBuilder.CreateIndex(
                name: "IX_Record_CognitiveID",
                table: "Record",
                column: "CognitiveID");

            migrationBuilder.CreateIndex(
                name: "IX_Record_EliminationID",
                table: "Record",
                column: "EliminationID");

            migrationBuilder.CreateIndex(
                name: "IX_Record_MobilityID",
                table: "Record",
                column: "MobilityID");

            migrationBuilder.CreateIndex(
                name: "IX_Record_NutritionID",
                table: "Record",
                column: "NutritionID");

            migrationBuilder.CreateIndex(
                name: "IX_Record_ProgressNoteID",
                table: "Record",
                column: "ProgressNoteID");

            migrationBuilder.CreateIndex(
                name: "IX_Record_SafetyID",
                table: "Record",
                column: "SafetyID");

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
                name: "FK_Record_Behaviour_BehaviourID",
                table: "Record",
                column: "BehaviourID",
                principalTable: "Behaviour",
                principalColumn: "BehaviourID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Cognitive_CognitiveID",
                table: "Record",
                column: "CognitiveID",
                principalTable: "Cognitive",
                principalColumn: "CognitiveID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Elimination_EliminationID",
                table: "Record",
                column: "EliminationID",
                principalTable: "Elimination",
                principalColumn: "EliminationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Mobility_MobilityID",
                table: "Record",
                column: "MobilityID",
                principalTable: "Mobility",
                principalColumn: "MobilityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Nutrition_NutritionID",
                table: "Record",
                column: "NutritionID",
                principalTable: "Nutrition",
                principalColumn: "NutritionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_ProgressNote_ProgressNoteID",
                table: "Record",
                column: "ProgressNoteID",
                principalTable: "ProgressNote",
                principalColumn: "ProgressNoteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Safety_SafetyID",
                table: "Record",
                column: "SafetyID",
                principalTable: "Safety",
                principalColumn: "SafetyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_SkinAndSensoryAids_SkinAndSensoryAidsId",
                table: "Record",
                column: "SkinAndSensoryAidsId",
                principalTable: "SkinAndSensoryAids",
                principalColumn: "SkinAndSensoryAidsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_ADLs_AdlsId1",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Behaviour_BehaviourID",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Cognitive_CognitiveID",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Elimination_EliminationID",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Mobility_MobilityID",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Nutrition_NutritionID",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_ProgressNote_ProgressNoteID",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Safety_SafetyID",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_SkinAndSensoryAids_SkinAndSensoryAidsId",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_AdlsId1",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_BehaviourID",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_CognitiveID",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_EliminationID",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_MobilityID",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_NutritionID",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_ProgressNoteID",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_SafetyID",
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
        }
    }
}
