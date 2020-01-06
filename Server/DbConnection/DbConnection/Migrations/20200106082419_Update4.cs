using Microsoft.EntityFrameworkCore.Migrations;

namespace DbConnection.Migrations
{
    public partial class Update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_AuthorFK",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Jokes_JokeFK",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AuthorFK",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_JokeFK",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JokeId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_JokeId",
                table: "Comments",
                column: "JokeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Jokes_JokeId",
                table: "Comments",
                column: "JokeId",
                principalTable: "Jokes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_AuthorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Jokes_JokeId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_JokeId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "JokeId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorFK",
                table: "Comments",
                column: "AuthorFK");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_JokeFK",
                table: "Comments",
                column: "JokeFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_AuthorFK",
                table: "Comments",
                column: "AuthorFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Jokes_JokeFK",
                table: "Comments",
                column: "JokeFK",
                principalTable: "Jokes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
