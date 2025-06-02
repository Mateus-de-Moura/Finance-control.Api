using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finance_control.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADD_COLUMN_RECURRENT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecurrent",
                table: "Revenues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Revenues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_UserId",
                table: "Revenues",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_Users_UserId",
                table: "Revenues",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_Users_UserId",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_UserId",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "IsRecurrent",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Revenues");
        }
    }
}
