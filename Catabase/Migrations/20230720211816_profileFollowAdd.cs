using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catabase.Migrations
{
    /// <inheritdoc />
    public partial class profileFollowAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddForeignKey(
            //    name: "FK_Follows_Profiles_ProfileId",
            //    table: "Follows",
            //    column: "ProfileId",
            //    principalTable: "Profiles",
            //    principalColumn: "ProfileId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddForeignKey(
            //    name: "FK_Follows_Profiles_ProfileId",
            //    table: "Follows",
            //    column: "ProfileId",
            //    principalTable: "Profiles",
            //    principalColumn: "ProfileId",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
