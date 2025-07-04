using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarModeloComIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Parcelas",
                table: "Parcelas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parcelas",
                table: "Parcelas",
                columns: new[] { "TituloNumero", "NumeroParcela" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Parcelas",
                table: "Parcelas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parcelas",
                table: "Parcelas",
                columns: new[] { "TituloNumero", "Vencimento" });
        }
    }
}
