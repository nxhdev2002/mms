using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateIFFQF3MM02IFFQF3MM07Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "IF_FQF3MM02",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    PlantCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    MaruCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ReceivingStockLine = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ProductionDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    PostingDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    PartCode = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    SpoiledPartsQuantity = table.Column<int>(type: "int", nullable: true),
                    SpoiledMaterialQuantity1 = table.Column<int>(type: "int", nullable: true),
                    MaterialCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    FreeShotQuantity = table.Column<int>(type: "int", nullable: true),
                    RecycledQuantity = table.Column<int>(type: "int", nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NormalCancelFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GrgiNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GrgiType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    MaterialDocType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    MaterialQuantity = table.Column<int>(type: "int", nullable: true),
                    SpoiledMaterialQuantity2 = table.Column<int>(type: "int", nullable: true),
                    RelatedPartReceiveNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RelatedGrType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    RelatedGrTransactionType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    InHousePartQuantityReceive = table.Column<int>(type: "int", nullable: true),
                    RelatedPartIssueNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RelatedGiType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    RelatedGiTransactionType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    RelatedInHousePartQuantityIssued = table.Column<int>(type: "int", nullable: true),
                    RelatedSpoiledPartQuantityIssued = table.Column<int>(type: "int", nullable: true),
                    Wip = table.Column<int>(type: "int", nullable: true),
                    ProductionId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    FinalPrice = table.Column<int>(type: "int", nullable: true),
                    Wbs = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    EarmarkedFund = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EarmarkedFundItem = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PsmsCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    GiUom = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
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
                    table.PrimaryKey("PK_IF_FQF3MM02", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IF_FQF3MM07",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CompanyBranch = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PostingKey = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    DocumentDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    PostingDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Plant = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ReferenceDocumentNo = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Order = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    GlAccount = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NormalCancelFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProfitCenter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Wbs = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    BaseUnitOfMeasure = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    AmountInLocalCurrency = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: true),
                    ExchangeRate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RefKey1 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    RefKey2 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    RefKey3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EarmarkFund = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EarmarkFundItem = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    MaterialNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    MainAssetNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    AssetSubNumber = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    TransType = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    EndingOfRecord = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
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
                    table.PrimaryKey("PK_IF_FQF3MM07", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IF_FQF3MM02");

            migrationBuilder.DropTable(
                name: "IF_FQF3MM07");

        }
    }
}
