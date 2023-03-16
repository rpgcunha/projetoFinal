using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class processo1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Tdoencas_DoencaId",
                table: "Processo");

            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Thospitais_HospitalId",
                table: "Processo");

            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Tmedicos_MedicoId",
                table: "Processo");

            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Tutentes_UtenteId",
                table: "Processo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Processo",
                table: "Processo");

            migrationBuilder.RenameTable(
                name: "Processo",
                newName: "Tprocessos");

            migrationBuilder.RenameIndex(
                name: "IX_Processo_UtenteId",
                table: "Tprocessos",
                newName: "IX_Tprocessos_UtenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Processo_MedicoId",
                table: "Tprocessos",
                newName: "IX_Tprocessos_MedicoId");

            migrationBuilder.RenameIndex(
                name: "IX_Processo_HospitalId",
                table: "Tprocessos",
                newName: "IX_Tprocessos_HospitalId");

            migrationBuilder.RenameIndex(
                name: "IX_Processo_DoencaId",
                table: "Tprocessos",
                newName: "IX_Tprocessos_DoencaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tprocessos",
                table: "Tprocessos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tprocessos_Tdoencas_DoencaId",
                table: "Tprocessos",
                column: "DoencaId",
                principalTable: "Tdoencas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tprocessos_Thospitais_HospitalId",
                table: "Tprocessos",
                column: "HospitalId",
                principalTable: "Thospitais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tprocessos_Tmedicos_MedicoId",
                table: "Tprocessos",
                column: "MedicoId",
                principalTable: "Tmedicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tprocessos_Tutentes_UtenteId",
                table: "Tprocessos",
                column: "UtenteId",
                principalTable: "Tutentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tprocessos_Tdoencas_DoencaId",
                table: "Tprocessos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tprocessos_Thospitais_HospitalId",
                table: "Tprocessos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tprocessos_Tmedicos_MedicoId",
                table: "Tprocessos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tprocessos_Tutentes_UtenteId",
                table: "Tprocessos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tprocessos",
                table: "Tprocessos");

            migrationBuilder.RenameTable(
                name: "Tprocessos",
                newName: "Processo");

            migrationBuilder.RenameIndex(
                name: "IX_Tprocessos_UtenteId",
                table: "Processo",
                newName: "IX_Processo_UtenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Tprocessos_MedicoId",
                table: "Processo",
                newName: "IX_Processo_MedicoId");

            migrationBuilder.RenameIndex(
                name: "IX_Tprocessos_HospitalId",
                table: "Processo",
                newName: "IX_Processo_HospitalId");

            migrationBuilder.RenameIndex(
                name: "IX_Tprocessos_DoencaId",
                table: "Processo",
                newName: "IX_Processo_DoencaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Processo",
                table: "Processo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Tdoencas_DoencaId",
                table: "Processo",
                column: "DoencaId",
                principalTable: "Tdoencas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Thospitais_HospitalId",
                table: "Processo",
                column: "HospitalId",
                principalTable: "Thospitais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Tmedicos_MedicoId",
                table: "Processo",
                column: "MedicoId",
                principalTable: "Tmedicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Tutentes_UtenteId",
                table: "Processo",
                column: "UtenteId",
                principalTable: "Tutentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
