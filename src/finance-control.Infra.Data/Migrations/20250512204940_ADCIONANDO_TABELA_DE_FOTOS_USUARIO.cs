using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finance_control.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADCIONANDO_TABELA_DE_FOTOS_USUARIO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhotosUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUser = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotosUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotosUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotosUsers_UserId",
                table: "PhotosUsers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotosUsers");
        }
    }
}
