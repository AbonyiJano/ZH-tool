using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZH_tool.Data.Migrations
{
    /// <inheritdoc />
    public partial class FeladatTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ertekelesek_tabla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MegoldasId = table.Column<int>(type: "integer", nullable: false),
                    Pontszam = table.Column<int>(type: "integer", nullable: true),
                    LLM_Visszajelzes = table.Column<string>(type: "text", nullable: false),
                    ErtekelesDatuma = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ertekelesek_tabla", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ertekelesek_tabla_megoldasok_tabla_MegoldasId",
                        column: x => x.MegoldasId,
                        principalTable: "megoldasok_tabla",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feladatok_tabla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneraltZhId = table.Column<int>(type: "integer", nullable: false),
                    FeladatCime = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Temakor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Leiras = table.Column<string>(type: "text", nullable: false),
                    Kompetenciak = table.Column<List<string>>(type: "text[]", nullable: false),
                    Pontozas = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NehezsegiSzint = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feladatok_tabla", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feladatok_tabla_generalt_zh_tabla_GeneraltZhId",
                        column: x => x.GeneraltZhId,
                        principalTable: "generalt_zh_tabla",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ertekelesek_tabla_MegoldasId",
                table: "ertekelesek_tabla",
                column: "MegoldasId");

            migrationBuilder.CreateIndex(
                name: "IX_feladatok_tabla_GeneraltZhId",
                table: "feladatok_tabla",
                column: "GeneraltZhId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ertekelesek_tabla");

            migrationBuilder.DropTable(
                name: "feladatok_tabla");
        }
    }
}
