using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class IFFQF3MM06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IF_FQF3MM06",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    AuthorizationGroup = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    MaterialType = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    MaterialCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    IndustrySector = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MaterialDescription = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    MaterialGroup = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    BaseUnitOfMeasure = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    FlagDeletionPlantLevel = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Plant = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    StorageLocation = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ProductGroup = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ProductPurpose = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProfitCenter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BatchManagement = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ReservedStock = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Residue = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    LotCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    MrpGroup = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    MrpType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ProcurementType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SpecialProcurement = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ProdStorLocation = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    RepetManufacturing = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    RemProfile = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DoNotCost = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    VarianceKey = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CostingLotSize = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    ProductionVersion = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    SpecialProcurementType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ValuationCategory = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ValuationType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ValuationClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PriceDetermination = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PriceControl = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    StandardPrice = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    MovingPrice = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    WithQtyStructure = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MaterialOrigin = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    OriginGroup = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    BasicDataText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Katashiki = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VehicleControlKatashiki = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ToyotaOrNonToyota = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CategoryOfGear = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    GoshiCar = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SeriesOfVehicles = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    DeliverPowerOfDrivingWheels = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    VehicleName = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    PriceUnit = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MaruCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_IF_FQF3MM06", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IF_FQF3MM06");
        }
    }
}
