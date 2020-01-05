using Microsoft.EntityFrameworkCore.Migrations;

namespace DbConnection.Migrations
{
    public partial class Update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Jokes_Content",
                table: "Jokes",
                column: "Content",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jokes_Content",
                table: "Jokes");
        }
    }
}
