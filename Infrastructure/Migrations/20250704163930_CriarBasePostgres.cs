using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarBasePostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Titulos",
                columns: table => new
                {
                    Numero = table.Column<string>(type: "text", nullable: false),
                    NomeDevedor = table.Column<string>(type: "text", nullable: false),
                    CPF = table.Column<string>(type: "text", nullable: false),
                    JurosMensal = table.Column<decimal>(type: "numeric", nullable: false),
                    MultaPercentual = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titulos", x => x.Numero);
                });

            migrationBuilder.CreateTable(
                name: "Parcelas",
                columns: table => new
                {
                    TituloNumero = table.Column<string>(type: "text", nullable: false),
                    Vencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    NumeroParcela = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcelas", x => new { x.TituloNumero, x.Vencimento });
                    table.ForeignKey(
                        name: "FK_Parcelas_Titulos_TituloNumero",
                        column: x => x.TituloNumero,
                        principalTable: "Titulos",
                        principalColumn: "Numero",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcelas");

            migrationBuilder.DropTable(
                name: "Titulos");
        }
    }
}
