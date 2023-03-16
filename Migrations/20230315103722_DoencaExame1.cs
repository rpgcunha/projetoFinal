using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class DoencaExame1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TdoencaExames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoencaId = table.Column<int>(type: "int", nullable: false),
                    ExameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TdoencaExames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TdoencaExames_Tdoencas_DoencaId",
                        column: x => x.DoencaId,
                        principalTable: "Tdoencas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TdoencaExames_Texames_ExameId",
                        column: x => x.ExameId,
                        principalTable: "Texames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TdoencaExames_DoencaId",
                table: "TdoencaExames",
                column: "DoencaId");

            migrationBuilder.CreateIndex(
                name: "IX_TdoencaExames_ExameId",
                table: "TdoencaExames",
                column: "ExameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TdoencaExames");
        }
    }
}
