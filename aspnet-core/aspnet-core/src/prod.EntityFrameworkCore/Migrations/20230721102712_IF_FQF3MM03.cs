using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class IFFQF3MM03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IF_FQF3MM03",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    DocumentDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CustomerPlantCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CustomerDockCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PartCategory = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    WithholdingTaxFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    WithholdingTaxRate = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PdsNo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PartReceivedDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SequenceDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SequenceNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ReferenceDocumentNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PostingDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SupplierCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SupplierPlantCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PartQuantity = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    UnitBuyingPrice = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    UnitBuyingAmount = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    UnitSellingPrice = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    UnitSellingAmount = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    PriceStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    TotalAmount = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    VatAmount = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    VatCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PaymentTerm = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ReasonCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MarkCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SignCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CancelFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SupplierInvoiceNo = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    TopasSmrNo = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    TopasSmrItemNo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CustomerBranch = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Wbs = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Asset = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    OrderReasonCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    RetroFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ValuationType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ConditionType = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ConditionTypeAmt = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: true),
                    PrepaidTaxAmt = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: true),
                    WithholdingTaxAmt = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: true),
                    StampFeeAmt = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: true),
                    GlAmount = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: true),
                    SptCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    HigherLevelItem = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    WithholdingTaxBaseAmt = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: true),
                    TypeOfSales = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ProfitCenter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DueDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    ItemText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EndingOfRecord = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_IF_FQF3MM03", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IF_FQF3MM03");
        }
    }
}
