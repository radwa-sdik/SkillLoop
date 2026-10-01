using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillLoop.Infrasturcture.Migrations
{
    public partial class auth_email_otp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneVerified",
                table: "Users",
                newName: "EmailVerified");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailVerified",
                table: "Users",
                newName: "PhoneVerified");
        }
    }
}