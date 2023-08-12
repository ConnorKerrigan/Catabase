using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catabase.Migrations
{
    /// <inheritdoc />
    public partial class modelstatenotvalid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "770b5281-3754-4702-b029-20ac28e04181", "AQAAAAIAAYagAAAAEDUmiCnpJpJWCaw2K/R4GY0dkWkmZ4jtWtVbhbna9orO3W3oXLjz4IwqrqlRmn6wOA==", "b7ab6df9-8ee4-49bb-a08c-cbdb7d2f5a9d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f77ff2f-f200-4cc0-aa99-ab66f953bf03", "AQAAAAIAAYagAAAAEF5CSs8eF7l2UsfeDQaHUBv5Z58gsCRmlZlALOvQTE3oiqtR2KfllCLaHKZozYgiOw==", "7c5044cb-4dd1-4a28-84a3-d08f58a2c070" });
        }
    }
}
