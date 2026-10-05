using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finance_control.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADJUSTING_EXPENSES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
               name: "active",
               table: "expenses",
               nullable: false
               );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "Active",
               table: "Expenses");
        }
    }
}
