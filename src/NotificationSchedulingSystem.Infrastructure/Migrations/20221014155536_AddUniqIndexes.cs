using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationSchedulingSystem.Infrastructure.Migrations
{
    public partial class AddUniqIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UC_Company_Name",
                table: "Companies",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UC_Company_Number",
                table: "Companies",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UC_Company_Name",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "UC_Company_Number",
                table: "Companies");
        }
    }
}
