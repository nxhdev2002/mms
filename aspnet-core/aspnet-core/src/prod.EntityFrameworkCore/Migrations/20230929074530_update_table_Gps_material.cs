using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updatetableGpsmaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PalletZ",
                table: "InvGpsMaterial",
                newName: "PalletR");

            migrationBuilder.RenameColumn(
                name: "PalletY",
                table: "InvGpsMaterial",
                newName: "PalletL");

            migrationBuilder.RenameColumn(
                name: "PalletX",
                table: "InvGpsMaterial",
                newName: "PalletH");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PalletR",
                table: "InvGpsMaterial",
                newName: "PalletZ");

            migrationBuilder.RenameColumn(
                name: "PalletL",
                table: "InvGpsMaterial",
                newName: "PalletY");

            migrationBuilder.RenameColumn(
                name: "PalletH",
                table: "InvGpsMaterial",
                newName: "PalletX");
        }
    }
}
