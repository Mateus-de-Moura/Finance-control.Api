using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finance_control.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADD_TABLE_EXPENSESCOMPROVANT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProofPath",
                table: "Expenses");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpensesComprovantId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpensesComprovant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesComprovant", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpensesComprovantId",
                table: "Expenses",
                column: "ExpensesComprovantId",
                unique: true,
                filter: "[ExpensesComprovantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpensesComprovant_ExpensesComprovantId",
                table: "Expenses",
                column: "ExpensesComprovantId",
                principalTable: "ExpensesComprovant",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpensesComprovant_ExpensesComprovantId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpensesComprovant");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpensesComprovantId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpensesComprovantId",
                table: "Expenses");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProofPath",
                table: "Expenses",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
