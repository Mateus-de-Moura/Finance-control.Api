using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finance_control.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADD_COLUM_DESCRIPTION_EXPENSES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Expenses");
        }
    }
}
