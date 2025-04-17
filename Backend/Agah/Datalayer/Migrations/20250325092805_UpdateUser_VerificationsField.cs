using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser_VerificationsField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationTokenExpiry",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneVerificationToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PhoneVerificationTokenExpiry",
                table: "User",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailVerificationTokenExpiry",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneVerificationToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneVerificationTokenExpiry",
                table: "User");
        }
    }
}
