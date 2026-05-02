using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeraConstrucoes.Infrastructure.Migrations
{
    public partial class CampoStatusNotaFiscal2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "NFCes",
                newName: "SituacaoNota");

            migrationBuilder.AddColumn<bool>(
                name: "StatusProcessamento",
                table: "NFCes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusProcessamento",
                table: "NFCes");

            migrationBuilder.RenameColumn(
                name: "SituacaoNota",
                table: "NFCes",
                newName: "Status");
        }
    }
}
