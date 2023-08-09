using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catabase.Migrations
{
    /// <inheritdoc />
    public partial class seedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {





            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1d4defef-a40d-41ef-a329-3a141369652e", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateCreated", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "305fde3f-e0f9-4994-959a-dfc5c53b5430", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CatabaseUser", null, false, false, null, null, "ADMIN", "AQAAAAIAAYagAAAAEJyW3qOu6ZhXQga+/w27arrr4/qzVRrVdFUKZARhxxZk5SG2/XBAXOljYS4yFlozUg==", null, false, "dc93ccd2-f0f6-4163-bbf5-5cfd72b3194b", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1d4defef-a40d-41ef-a329-3a141369652e", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1d4defef-a40d-41ef-a329-3a141369652e", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d4defef-a40d-41ef-a329-3a141369652e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");






        }
    }
}
