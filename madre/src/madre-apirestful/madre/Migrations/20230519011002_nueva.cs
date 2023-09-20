using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace madre.Migrations
{
    /// <inheritdoc />
    public partial class nueva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComputadorasHijas",
                columns: table => new
                {
                    Ip = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Permiso = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputadorasHijas", x => x.Ip);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Errores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ErrorCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errores", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RegistrosProgramas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombrePrograma = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    Usuario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ventana = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TituloVentana = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Inicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ComputadoraHijaIp = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosProgramas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosProgramas_ComputadorasHijas_ComputadoraHijaIp",
                        column: x => x.ComputadoraHijaIp,
                        principalTable: "ComputadorasHijas",
                        principalColumn: "Ip",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrosProgramas_Errores_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ErroresRegistros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RegistroProgramaId = table.Column<int>(type: "int", nullable: false),
                    ErrorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErroresRegistros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErroresRegistros_Errores_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ErroresRegistros_RegistrosProgramas_RegistroProgramaId",
                        column: x => x.RegistroProgramaId,
                        principalTable: "RegistrosProgramas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ComputadorasHijas",
                columns: new[] { "Ip", "Nombre", "Permiso", "Status" },
                values: new object[,]
                {
                    { "192.168.1.210:7050", "Computadora 1", false, false },
                    { "192.168.1.211:7051", "Computadora 2", false, false },
                    { "192.168.1.212:7052", "Computadora 3", false, false },
                    { "192.168.1.213:7053", "Computadora 4", false, false },
                    { "192.168.1.214:7054", "Computadora 5", false, false },
                    { "192.168.1.215:7055", "Computadora 6", false, false },
                    { "192.168.1.216:7056", "Computadora 7", false, false },
                    { "192.168.1.217:7057", "Computadora 8", false, false },
                    { "192.168.1.218:7058", "Computadora 9", false, false },
                    { "192.168.1.219:7059", "Computadora 10", false, false },
                    { "192.168.1.220:7060", "Computadora 11", false, false }
                });

            migrationBuilder.InsertData(
                table: "Errores",
                columns: new[] { "Id", "Descripcion", "ErrorCode" },
                values: new object[,]
                {
                    { 1, "Error 1: causa impredecible", "ERR001" },
                    { 2, "Error 2: causa imposible", "ERR002" },
                    { 3, "Error 3: causa improvable", "ERR003" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErroresRegistros_ErrorId",
                table: "ErroresRegistros",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_ErroresRegistros_RegistroProgramaId",
                table: "ErroresRegistros",
                column: "RegistroProgramaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosProgramas_ComputadoraHijaIp",
                table: "RegistrosProgramas",
                column: "ComputadoraHijaIp");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosProgramas_ErrorId",
                table: "RegistrosProgramas",
                column: "ErrorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErroresRegistros");

            migrationBuilder.DropTable(
                name: "RegistrosProgramas");

            migrationBuilder.DropTable(
                name: "ComputadorasHijas");

            migrationBuilder.DropTable(
                name: "Errores");
        }
    }
}
