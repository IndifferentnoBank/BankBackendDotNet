using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class new_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Service",
                table: "FireBaseTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Service",
                table: "FireBaseTokens");
        }
    }
}
