using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutilizador_Tmedicos_MedicoId",
                table: "Tutilizador");

            migrationBuilder.DropIndex(
                name: "IX_Tutilizador_MedicoId",
                table: "Tutilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Tmedicos_UtilizadorId",
                table: "Tmedicos",
                column: "UtilizadorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tmedicos_Tutilizador_UtilizadorId",
                table: "Tmedicos",
                column: "UtilizadorId",
                principalTable: "Tutilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tmedicos_Tutilizador_UtilizadorId",
                table: "Tmedicos");

            migrationBuilder.DropIndex(
                name: "IX_Tmedicos_UtilizadorId",
                table: "Tmedicos");

            migrationBuilder.CreateIndex(
                name: "IX_Tutilizador_MedicoId",
                table: "Tutilizador",
                column: "MedicoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutilizador_Tmedicos_MedicoId",
                table: "Tutilizador",
                column: "MedicoId",
                principalTable: "Tmedicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
