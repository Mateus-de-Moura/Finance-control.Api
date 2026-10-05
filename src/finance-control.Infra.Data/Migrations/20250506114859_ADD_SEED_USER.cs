using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finance_control.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADD_SEED_USER : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordSalt", "RefreshToken", "RefreshTokenExpirationTime", "Surname", "UserName" },
                values: new object[] { new Guid("c3d2251f-1e0b-42b6-8868-75d03046460c"), "admin@admin.com", "Admin", "$2a$11$kgueTQbW2exSJwFqWxQ.h.cFK5l5WArN8DdGWCLS1UZ849lop2C2m", "$2a$11$kgueTQbW2exSJwFqWxQ.h.", "vMVEc5sypGQDpoqFWtmXOuVfPwjzEo9EuorBukiH/WbE2EYvAeGJxaCBGnwgRv7sSV2/6dfX220TjC4quGC/MexPfZiL/U6YPferYZRGcPz30fFg4jzO4Y1wTbXSvV2ta5j8nlAhdvGDT0dTW42RgTmrzmKun4B0nPCV3AIpupQ=", new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c3d2251f-1e0b-42b6-8868-75d03046460c"));
        }
    }
}
