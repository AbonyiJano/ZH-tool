using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZH_tool.Data.Migrations
{
    /// <inheritdoc />
    public partial class HallgatoMegoldas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hallgatok_tabla",
                columns: table => new
                {
                    Neptunkod = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    Nev = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hallgatok_tabla", x => x.Neptunkod);
                });

            migrationBuilder.CreateTable(
                name: "megoldasok_tabla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HallgatoNeptunkod = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    GeneraltZhId = table.Column<int>(type: "integer", nullable: false),
                    BekuldottMegoldas = table.Column<string>(type: "text", nullable: false),
                    BekuldesDatuma = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_megoldasok_tabla", x => x.Id);
                    table.ForeignKey(
                        name: "FK_megoldasok_tabla_generalt_zh_tabla_GeneraltZhId",
                        column: x => x.GeneraltZhId,
                        principalTable: "generalt_zh_tabla",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_megoldasok_tabla_hallgatok_tabla_HallgatoNeptunkod",
                        column: x => x.HallgatoNeptunkod,
                        principalTable: "hallgatok_tabla",
                        principalColumn: "Neptunkod",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_megoldasok_tabla_GeneraltZhId",
                table: "megoldasok_tabla",
                column: "GeneraltZhId");

            migrationBuilder.CreateIndex(
                name: "IX_megoldasok_tabla_HallgatoNeptunkod",
                table: "megoldasok_tabla",
                column: "HallgatoNeptunkod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "megoldasok_tabla");

            migrationBuilder.DropTable(
                name: "hallgatok_tabla");
        }
    }
}
