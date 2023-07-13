using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAuthenAndTextMessage.Migrations
{
    /// <inheritdoc />
    public partial class addDeleteForMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleteForEveryOne",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelfDelete",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleteForEveryOne",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsSelfDelete",
                table: "Messages");
        }
    }
}
