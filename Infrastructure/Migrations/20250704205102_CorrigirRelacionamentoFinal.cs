using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirRelacionamentoFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parcelas_Titulos_TituloNumero",
                table: "Parcelas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Titulos",
                table: "Titulos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parcelas",
                table: "Parcelas");

            migrationBuilder.DropColumn(
                name: "TituloNumero",
                table: "Parcelas");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Titulos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Parcelas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TituloId",
                table: "Parcelas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Titulos",
                table: "Titulos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parcelas",
                table: "Parcelas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Parcelas_TituloId",
                table: "Parcelas",
                column: "TituloId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcelas_Titulos_TituloId",
                table: "Parcelas",
                column: "TituloId",
                principalTable: "Titulos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parcelas_Titulos_TituloId",
                table: "Parcelas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Titulos",
                table: "Titulos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parcelas",
                table: "Parcelas");

            migrationBuilder.DropIndex(
                name: "IX_Parcelas_TituloId",
                table: "Parcelas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Titulos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Parcelas");

            migrationBuilder.DropColumn(
                name: "TituloId",
                table: "Parcelas");

            migrationBuilder.AddColumn<string>(
                name: "TituloNumero",
                table: "Parcelas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Titulos",
                table: "Titulos",
                column: "Numero");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parcelas",
                table: "Parcelas",
                columns: new[] { "TituloNumero", "NumeroParcela" });

            migrationBuilder.AddForeignKey(
                name: "FK_Parcelas_Titulos_TituloNumero",
                table: "Parcelas",
                column: "TituloNumero",
                principalTable: "Titulos",
                principalColumn: "Numero",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
