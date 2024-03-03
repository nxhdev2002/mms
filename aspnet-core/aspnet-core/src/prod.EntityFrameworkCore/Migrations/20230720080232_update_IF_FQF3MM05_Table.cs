using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateIFFQF3MM05Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "IF_FQF3MM05",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunningNo = table.Column<int>(type: "int", nullable: true),
                    DocumentDate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PostingDate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DocumentHeaderText = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MovementType = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    MaterialCodeFrom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PlantFrom = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ValuationTypeFrom = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    StorageLocationFrom = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ProductionVersion = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    UnitOfEntry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ItemText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GlAccount = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Wbs = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    MaterialCodeTo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PlantTo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ValuationTypeTo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    StorageLocationTo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    BfPc = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CancelFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ReffMatDocNo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    VendorNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ProfitCenter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ShipemntCat = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    AssetNo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    SubAssetNo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    EndOfRecord = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_IF_FQF3MM05", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IF_FQF3MM05");

         
        }
    }
}
