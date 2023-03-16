using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class DoencaSintomas1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TdoencaSintomas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoencaId = table.Column<int>(type: "int", nullable: false),
                    SintomaId = table.Column<int>(type: "int", nullable: false),
                    Relevancia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TdoencaSintomas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TdoencaSintomas_Tdoencas_DoencaId",
                        column: x => x.DoencaId,
                        principalTable: "Tdoencas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TdoencaSintomas_Tsintomas_SintomaId",
                        column: x => x.SintomaId,
                        principalTable: "Tsintomas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TdoencaSintomas_DoencaId",
                table: "TdoencaSintomas",
                column: "DoencaId");

            migrationBuilder.CreateIndex(
                name: "IX_TdoencaSintomas_SintomaId",
                table: "TdoencaSintomas",
                column: "SintomaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TdoencaSintomas");
        }
    }
}
