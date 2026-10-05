using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finance_control.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADD_TABLE_LOGINLOCATIONDATA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PhotosUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "LoginLocationData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailRequest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLocationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLocationData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginLocationData_UserId",
                table: "LoginLocationData",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginLocationData");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "PhotosUsers");
        }
    }
}
