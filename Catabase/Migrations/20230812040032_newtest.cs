using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catabase.Migrations
{
    /// <inheritdoc />
    public partial class newtest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "206e9119-0c0b-46ac-8bbc-4f5988682a19", "AQAAAAIAAYagAAAAECSAtuWGvX5UnNUEgOOTL887rJZBVqsIfjKGwO14botCc6xF2BK+VWTMbcaltBsdnw==", "942259ed-0605-4960-9bcb-fe6544cc64fd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "770b5281-3754-4702-b029-20ac28e04181", "AQAAAAIAAYagAAAAEDUmiCnpJpJWCaw2K/R4GY0dkWkmZ4jtWtVbhbna9orO3W3oXLjz4IwqrqlRmn6wOA==", "b7ab6df9-8ee4-49bb-a08c-cbdb7d2f5a9d" });
        }
    }
}
