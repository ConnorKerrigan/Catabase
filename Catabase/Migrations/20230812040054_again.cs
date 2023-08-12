using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catabase.Migrations
{
    /// <inheritdoc />
    public partial class again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "043bee8a-72fc-46b6-b840-7d7c2684732a", "AQAAAAIAAYagAAAAENbr7d3UppZuGwMKWDdtMc3WLybndayJce121DQtYTk+V93jZtzKQ500mP1I1E0caA==", "92eace2e-b548-4b08-9b15-4a601859b225" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "206e9119-0c0b-46ac-8bbc-4f5988682a19", "AQAAAAIAAYagAAAAECSAtuWGvX5UnNUEgOOTL887rJZBVqsIfjKGwO14botCc6xF2BK+VWTMbcaltBsdnw==", "942259ed-0605-4960-9bcb-fe6544cc64fd" });
        }
    }
}
