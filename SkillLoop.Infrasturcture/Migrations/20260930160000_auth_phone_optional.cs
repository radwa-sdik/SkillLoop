using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillLoop.Infrasturcture.Migrations
{
    public partial class auth_phone_optional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Users_PhoneNumber", table: "Users");
            migrationBuilder.AlterColumn<string>(name: "PhoneNumber", table: "Users", type: "nvarchar(30)", maxLength: 30, nullable: true, oldClrType: typeof(string), oldType: "nvarchar(30)", oldMaxLength: 30);
            migrationBuilder.CreateIndex(name: "IX_Users_PhoneNumber", table: "Users", column: "PhoneNumber", unique: true, filter: "[PhoneNumber] IS NOT NULL");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Users_PhoneNumber", table: "Users");
            migrationBuilder.AlterColumn<string>(name: "PhoneNumber", table: "Users", type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "", oldClrType: typeof(string), oldType: "nvarchar(30)", oldMaxLength: 30, oldNullable: true);
            migrationBuilder.CreateIndex(name: "IX_Users_PhoneNumber", table: "Users", column: "PhoneNumber", unique: true);
        }
    }
}
