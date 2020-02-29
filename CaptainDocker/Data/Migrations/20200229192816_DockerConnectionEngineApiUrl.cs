using Microsoft.EntityFrameworkCore.Migrations;

namespace CaptainDocker.Data.Migrations
{
    public partial class DockerConnectionEngineApiUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EngineApiUrl",
                table: "DockerConnections",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineApiUrl",
                table: "DockerConnections");
        }
    }
}
