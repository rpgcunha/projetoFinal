using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TcatDoenca",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcatDoenca", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TcatExame",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcatExame", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TcatSintoma",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcatSintoma", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Thospital",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cidade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thospital", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tmedico",
                columns: table => new
                {
                    bi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    especialidade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tmedico", x => x.bi);
                });

            migrationBuilder.CreateTable(
                name: "Tutente",
                columns: table => new
                {
                    numeroUtente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dataNascimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    genero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cidade = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutente", x => x.numeroUtente);
                });

            migrationBuilder.CreateTable(
                name: "Tdoenca",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    catDoencaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tdoenca", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tdoenca_TcatDoenca_catDoencaId",
                        column: x => x.catDoencaId,
                        principalTable: "TcatDoenca",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Texame",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    catExameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texame", x => x.id);
                    table.ForeignKey(
                        name: "FK_Texame_TcatExame_catExameId",
                        column: x => x.catExameId,
                        principalTable: "TcatExame",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tsintoma",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    catSintomaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tsintoma", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tsintoma_TcatSintoma_catSintomaId",
                        column: x => x.catSintomaId,
                        principalTable: "TcatSintoma",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TprocessoUtente",
                columns: table => new
                {
                    processo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    utenteId = table.Column<int>(type: "int", nullable: false),
                    medicoBi = table.Column<int>(type: "int", nullable: false),
                    hospitalId = table.Column<int>(type: "int", nullable: false),
                    dataAbertura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dataFecho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    doencaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TprocessoUtente", x => x.processo);
                    table.ForeignKey(
                        name: "FK_TprocessoUtente_Tdoenca_doencaId",
                        column: x => x.doencaId,
                        principalTable: "Tdoenca",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TprocessoUtente_Thospital_hospitalId",
                        column: x => x.hospitalId,
                        principalTable: "Thospital",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TprocessoUtente_Tmedico_medicoBi",
                        column: x => x.medicoBi,
                        principalTable: "Tmedico",
                        principalColumn: "bi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TprocessoUtente_Tutente_utenteId",
                        column: x => x.utenteId,
                        principalTable: "Tutente",
                        principalColumn: "numeroUtente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TdoencaExame",
                columns: table => new
                {
                    doencaId = table.Column<int>(type: "int", nullable: false),
                    exameId = table.Column<int>(type: "int", nullable: false),
                    relevancia = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TdoencaExame", x => new { x.doencaId, x.exameId });
                    table.ForeignKey(
                        name: "FK_TdoencaExame_Tdoenca_doencaId",
                        column: x => x.doencaId,
                        principalTable: "Tdoenca",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TdoencaExame_Texame_exameId",
                        column: x => x.exameId,
                        principalTable: "Texame",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TdoencaSintoma",
                columns: table => new
                {
                    doencaId = table.Column<int>(type: "int", nullable: false),
                    sintomaId = table.Column<int>(type: "int", nullable: false),
                    relevancia = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TdoencaSintoma", x => new { x.doencaId, x.sintomaId });
                    table.ForeignKey(
                        name: "FK_TdoencaSintoma_Tdoenca_doencaId",
                        column: x => x.doencaId,
                        principalTable: "Tdoenca",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TdoencaSintoma_Tsintoma_sintomaId",
                        column: x => x.sintomaId,
                        principalTable: "Tsintoma",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TprocessoExame",
                columns: table => new
                {
                    processoId = table.Column<int>(type: "int", nullable: false),
                    exameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TprocessoExame", x => new { x.processoId, x.exameId });
                    table.ForeignKey(
                        name: "FK_TprocessoExame_Texame_exameId",
                        column: x => x.exameId,
                        principalTable: "Texame",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TprocessoExame_TprocessoUtente_processoId",
                        column: x => x.processoId,
                        principalTable: "TprocessoUtente",
                        principalColumn: "processo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TprocessoSintoma",
                columns: table => new
                {
                    processoId = table.Column<int>(type: "int", nullable: false),
                    sintomaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TprocessoSintoma", x => new { x.processoId, x.sintomaId });
                    table.ForeignKey(
                        name: "FK_TprocessoSintoma_TprocessoUtente_processoId",
                        column: x => x.processoId,
                        principalTable: "TprocessoUtente",
                        principalColumn: "processo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TprocessoSintoma_Tsintoma_sintomaId",
                        column: x => x.sintomaId,
                        principalTable: "Tsintoma",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tdoenca_catDoencaId",
                table: "Tdoenca",
                column: "catDoencaId");

            migrationBuilder.CreateIndex(
                name: "IX_TdoencaExame_exameId",
                table: "TdoencaExame",
                column: "exameId");

            migrationBuilder.CreateIndex(
                name: "IX_TdoencaSintoma_sintomaId",
                table: "TdoencaSintoma",
                column: "sintomaId");

            migrationBuilder.CreateIndex(
                name: "IX_Texame_catExameId",
                table: "Texame",
                column: "catExameId");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoExame_exameId",
                table: "TprocessoExame",
                column: "exameId");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoSintoma_sintomaId",
                table: "TprocessoSintoma",
                column: "sintomaId");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoUtente_doencaId",
                table: "TprocessoUtente",
                column: "doencaId");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoUtente_hospitalId",
                table: "TprocessoUtente",
                column: "hospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoUtente_medicoBi",
                table: "TprocessoUtente",
                column: "medicoBi");

            migrationBuilder.CreateIndex(
                name: "IX_TprocessoUtente_utenteId",
                table: "TprocessoUtente",
                column: "utenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tsintoma_catSintomaId",
                table: "Tsintoma",
                column: "catSintomaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TdoencaExame");

            migrationBuilder.DropTable(
                name: "TdoencaSintoma");

            migrationBuilder.DropTable(
                name: "TprocessoExame");

            migrationBuilder.DropTable(
                name: "TprocessoSintoma");

            migrationBuilder.DropTable(
                name: "Texame");

            migrationBuilder.DropTable(
                name: "TprocessoUtente");

            migrationBuilder.DropTable(
                name: "Tsintoma");

            migrationBuilder.DropTable(
                name: "TcatExame");

            migrationBuilder.DropTable(
                name: "Tdoenca");

            migrationBuilder.DropTable(
                name: "Thospital");

            migrationBuilder.DropTable(
                name: "Tmedico");

            migrationBuilder.DropTable(
                name: "Tutente");

            migrationBuilder.DropTable(
                name: "TcatSintoma");

            migrationBuilder.DropTable(
                name: "TcatDoenca");
        }
    }
}
