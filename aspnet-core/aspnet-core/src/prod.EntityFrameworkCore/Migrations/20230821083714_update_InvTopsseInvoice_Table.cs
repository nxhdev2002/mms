using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateInvTopsseInvoiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvTopsseInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistFd = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BillOfLading = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Etd = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_InvTopsseInvoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvTopsseInvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    OrderNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    ItemNo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PartQty = table.Column<int>(type: "int", nullable: true),
                    UnitFob = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TariffCd = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    HsCd = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_InvTopsseInvoiceDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvTopsseInvoice");

            migrationBuilder.DropTable(
                name: "InvTopsseInvoiceDetails");
        }
    }
}
