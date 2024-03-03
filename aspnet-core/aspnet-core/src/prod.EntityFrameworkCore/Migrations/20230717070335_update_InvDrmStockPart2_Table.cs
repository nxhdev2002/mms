using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateInvDrmStockPart2Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cfc",
                table: "InvDrmStockPart_T",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartCode",
                table: "InvDrmStockPart_T",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierNo",
                table: "InvDrmStockPart_T",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cfc",
                table: "InvDrmStockPart_T");

            migrationBuilder.DropColumn(
                name: "PartCode",
                table: "InvDrmStockPart_T");

            migrationBuilder.DropColumn(
                name: "SupplierNo",
                table: "InvDrmStockPart_T");
        }
    }
}
