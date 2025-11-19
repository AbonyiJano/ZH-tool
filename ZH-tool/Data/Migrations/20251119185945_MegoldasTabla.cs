using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZH_tool.Data.Migrations
{
    /// <inheritdoc />
    public partial class MegoldasTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LLM_Visszajelzes",
                table: "ertekelesek_tabla",
                newName: "LLMVisszajelzes");

            migrationBuilder.AddColumn<string>(
                name: "MintaMegoldas",
                table: "feladatok_tabla",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OsszPontszam",
                table: "ertekelesek_tabla",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MintaMegoldas",
                table: "feladatok_tabla");

            migrationBuilder.DropColumn(
                name: "OsszPontszam",
                table: "ertekelesek_tabla");

            migrationBuilder.RenameColumn(
                name: "LLMVisszajelzes",
                table: "ertekelesek_tabla",
                newName: "LLM_Visszajelzes");
        }
    }
}
