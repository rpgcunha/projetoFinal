using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class Exames1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TcatExames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcatExames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Texames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CatExameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Texames_TcatExames_CatExameId",
                        column: x => x.CatExameId,
                        principalTable: "TcatExames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Texames_CatExameId",
                table: "Texames",
                column: "CatExameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Texames");

            migrationBuilder.DropTable(
                name: "TcatExames");
        }
    }
}
