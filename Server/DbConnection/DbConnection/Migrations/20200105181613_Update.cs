using Microsoft.EntityFrameworkCore.Migrations;

namespace DbConnection.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_AuthorFK",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Joke_JokeFK",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Joke_Users_AuthorFK",
                table: "Joke");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Joke",
                table: "Joke");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Joke",
                newName: "Jokes");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Joke_AuthorFK",
                table: "Jokes",
                newName: "IX_Jokes_AuthorFK");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_JokeFK",
                table: "Comments",
                newName: "IX_Comments_JokeFK");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_AuthorFK",
                table: "Comments",
                newName: "IX_Comments_AuthorFK");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jokes",
                table: "Jokes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname",
                table: "Users",
                column: "Nickname",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Jokes_Users_AuthorFK",
                table: "Jokes",
                column: "AuthorFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_AuthorFK",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Jokes_JokeFK",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Jokes_Users_AuthorFK",
                table: "Jokes");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Nickname",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jokes",
                table: "Jokes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Jokes",
                newName: "Joke");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Jokes_AuthorFK",
                table: "Joke",
                newName: "IX_Joke_AuthorFK");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_JokeFK",
                table: "Comment",
                newName: "IX_Comment_JokeFK");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AuthorFK",
                table: "Comment",
                newName: "IX_Comment_AuthorFK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Joke",
                table: "Joke",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_AuthorFK",
                table: "Comment",
                column: "AuthorFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Joke_JokeFK",
                table: "Comment",
                column: "JokeFK",
                principalTable: "Joke",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Joke_Users_AuthorFK",
                table: "Joke",
                column: "AuthorFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
