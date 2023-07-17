using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAuthenAndTextMessage.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "Avatar", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "65d192e6-15ec-47b8-a82d-43bbe0d95dfe", "AQAAAAIAAYagAAAAEJ9yxvEpsZrGYLya1WtEeRarOxL9BoKcUoI6B/lTEe1NcFRJlYK+PE0UDf7LgrMitA==", "45fe7d53-dbc2-44b1-b388-ff47d4f3601f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "72a74df4-c3f3-4bd9-8f8d-2d2bcb7320e6", "AQAAAAIAAYagAAAAECf1ByyD1CSlrjNWUzluS0Jp+qn4TUv3t0gXF/FP1LSS8hea4Cpc+efHmJL6yesX8w==", "51f6462c-c197-4a43-aec9-e7ffffafe150" });
        }
    }
}
