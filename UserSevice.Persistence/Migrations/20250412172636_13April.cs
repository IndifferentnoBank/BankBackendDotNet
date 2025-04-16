﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class _13April : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
