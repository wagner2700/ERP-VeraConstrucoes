using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeraConstrucoes.Infrastructure.Migrations
{
    public partial class NCMProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ncm",
                table: "Produtos",
                type: "varchar(8)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NCM",
                columns: table => new
                {
                    Ncm = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descricao = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    csosn = table.Column<int>(type: "int(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NCM", x => x.Ncm);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Ncm",
                table: "Produtos",
                column: "Ncm");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_NCM_Ncm",
                table: "Produtos",
                column: "Ncm",
                principalTable: "NCM",
                principalColumn: "Ncm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_NCM_Ncm",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "NCM");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ncm",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Ncm",
                table: "Produtos");
        }
    }
}
