using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apoio_decisao_medica.Migrations
{
    /// <inheritdoc />
    public partial class processo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tprocessos_Tdoencas_DoencaId",
                table: "Tprocessos");

            migrationBuilder.AlterColumn<int>(
                name: "DoencaId",
                table: "Tprocessos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DataHoraFecho",
                table: "Tprocessos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Tprocessos_Tdoencas_DoencaId",
                table: "Tprocessos",
                column: "DoencaId",
                principalTable: "Tdoencas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tprocessos_Tdoencas_DoencaId",
                table: "Tprocessos");

            migrationBuilder.AlterColumn<int>(
                name: "DoencaId",
                table: "Tprocessos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataHoraFecho",
                table: "Tprocessos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tprocessos_Tdoencas_DoencaId",
                table: "Tprocessos",
                column: "DoencaId",
                principalTable: "Tdoencas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
