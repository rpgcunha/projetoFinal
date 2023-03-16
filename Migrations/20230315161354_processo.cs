using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class processo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroProcesso = table.Column<int>(type: "int", nullable: false),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    DataHoraAbertura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataHoraFecho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoencaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processo_Tdoencas_DoencaId",
                        column: x => x.DoencaId,
                        principalTable: "Tdoencas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processo_Thospitais_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Thospitais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processo_Tmedicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Tmedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processo_Tutentes_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Tutentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processo_DoencaId",
                table: "Processo",
                column: "DoencaId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_HospitalId",
                table: "Processo",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_MedicoId",
                table: "Processo",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_UtenteId",
                table: "Processo",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processo");
        }
    }
}
