using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateInvDrmImportPlans1Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvDrmImportPlan",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Etd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Eta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShipmentNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Cfc = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PartCode = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    PackingMonth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Ata = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_InvDrmImportPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvDrmStockPart_T",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MaterialCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    MaterialSpec = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DrmMaterialId = table.Column<long>(type: "bigint", nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PartId = table.Column<long>(type: "bigint", nullable: true),
                    MaterialId = table.Column<long>(type: "bigint", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    WorkingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
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
                    table.PrimaryKey("PK_InvDrmStockPart_T", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvDrmImportPlan");

            migrationBuilder.DropTable(
                name: "InvDrmStockPart_T");
        }
    }
}
