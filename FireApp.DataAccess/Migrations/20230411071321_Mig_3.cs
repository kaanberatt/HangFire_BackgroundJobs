using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireApp.DataAccess.Migrations
{
    public partial class Mig_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "IntegrationLog",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "IntegrationLog");
        }
    }
}
