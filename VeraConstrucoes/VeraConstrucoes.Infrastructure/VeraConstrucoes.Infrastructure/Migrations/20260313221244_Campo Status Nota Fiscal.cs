using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeraConstrucoes.Infrastructure.Migrations
{
    public partial class CampoStatusNotaFiscal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sucesso",
                table: "NFCes");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "NFCes",
                type: "varchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "NFCes");

            migrationBuilder.AddColumn<bool>(
                name: "Sucesso",
                table: "NFCes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
