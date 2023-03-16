using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class ProcessosExamesSintomas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TprocessoExames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessoId = table.Column<int>(type: "int", nullable: false),
                    ExameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TprocessoExames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TprocessoExames_Texames_ExameId",
                        column: x => x.ExameId,
                        principalTable: "Texames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TprocessoExames_Tprocessos_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Tprocessos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TprocessoSintomas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessoId = table.Column<int>(type: "int", nullable: false),
                    SintomaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TprocessoSintomas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TprocessoSintomas_Tprocessos_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Tprocessos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TprocessoSintomas_Tsintomas_SintomaId",
                        column: x => x.SintomaId,
                        principalTable: "Tsintomas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoExames_ExameId",
                table: "TprocessoExames",
                column: "ExameId");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoExames_ProcessoId",
                table: "TprocessoExames",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoSintomas_ProcessoId",
                table: "TprocessoSintomas",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoSintomas_SintomaId",
                table: "TprocessoSintomas",
                column: "SintomaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TprocessoExames");

            migrationBuilder.DropTable(
                name: "TprocessoSintomas");
        }
    }
}
