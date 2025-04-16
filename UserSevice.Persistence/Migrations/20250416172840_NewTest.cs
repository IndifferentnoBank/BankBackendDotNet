using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpiredTokens",
                columns: table => new
                {
                    Key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiredTokens", x => x.Key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpiredTokens");
        }
    }
}
