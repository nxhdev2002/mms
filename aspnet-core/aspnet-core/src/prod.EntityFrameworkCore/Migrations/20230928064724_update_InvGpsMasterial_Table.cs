using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateInvGpsMasterialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "InvGpsMasterial",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PartNameVn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PartType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Uses = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Spec = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsExpDate = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PartGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PriceConvert = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Uom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LocalImport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LeadTime = table.Column<int>(type: "int", nullable: true),
                    LeadTimeForecas = table.Column<int>(type: "int", nullable: true),
                    MinLot = table.Column<int>(type: "int", nullable: true),
                    BoxQty = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PalletX = table.Column<int>(type: "int", nullable: true),
                    PalletY = table.Column<int>(type: "int", nullable: true),
                    PalletZ = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvGpsMasterial", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvGpsMasterial");

        
        }
    }
}
