using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZH_tool.Data.Migrations
{
    /// <inheritdoc />
    public partial class GeneratedZhTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "generalt_zh_tabla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentZhId = table.Column<int>(type: "integer", nullable: false),
                    GeneratedJson = table.Column<string>(type: "text", nullable: false),
                    GenerationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generalt_zh_tabla", x => x.Id);
                    table.ForeignKey(
                        name: "FK_generalt_zh_tabla_zh_tabla_ParentZhId",
                        column: x => x.ParentZhId,
                        principalTable: "zh_tabla",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_generalt_zh_tabla_ParentZhId",
                table: "generalt_zh_tabla",
                column: "ParentZhId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "generalt_zh_tabla");
        }
    }
}
