using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillLoop.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneVerified",
                table: "Users",
                newName: "EmailVerified");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailVerified",
                table: "Users",
                newName: "PhoneVerified");
        }
    }
}
