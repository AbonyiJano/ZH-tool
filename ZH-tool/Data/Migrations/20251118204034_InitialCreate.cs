using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZH_tool.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "zh_tabla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nev = table.Column<string>(type: "text", nullable: false),
                    MintaZH = table.Column<string>(type: "text", nullable: false),
                    Tematika = table.Column<string>(type: "text", nullable: false),
                    TemakorLeiras = table.Column<string>(type: "text", nullable: false),
                    FeladatokSzama = table.Column<int>(type: "integer", nullable: false),
                    ProgramozasiNyelv = table.Column<string>(type: "text", nullable: false),
                    Nehezseg = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zh_tabla", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zh_tabla");
        }
    }
}
