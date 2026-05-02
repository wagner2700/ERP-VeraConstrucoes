using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeraConstrucoes.Infrastructure.Migrations
{
    public partial class NumerodoLote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumeroLote",
                table: "ConfiguracoesNFCe",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroLote",
                table: "ConfiguracoesNFCe");
        }
    }
}
