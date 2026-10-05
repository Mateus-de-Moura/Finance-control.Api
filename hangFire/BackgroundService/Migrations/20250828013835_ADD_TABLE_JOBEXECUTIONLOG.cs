using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackgroundService.Migrations
{
    /// <inheritdoc />
    public partial class ADD_TABLE_JOBEXECUTIONLOG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {        
            migrationBuilder.CreateTable(
                name: "JobExecutionLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Last_Execution = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobExecutionLog", x => x.Id);
                });           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {        
            migrationBuilder.DropTable(
                name: "JobExecutionLog");
        }
    }
}
