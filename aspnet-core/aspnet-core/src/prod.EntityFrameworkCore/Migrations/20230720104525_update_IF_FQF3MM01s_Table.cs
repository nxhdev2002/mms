using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateIFFQF3MM01sTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "IF_FQF3MM01",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    Vin = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    Urn = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SpecSheetNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    IdLine = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Katashiki = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SaleKatashiki = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SaleSuffix = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Spec200Digits = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProductionSuffix = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    LotCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    EnginePrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    EngineNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PlantCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    LineOffDatetime = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    InteriorColor = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ExteriorColor = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DestinationCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    EdOdno = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CancelFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SmsCarFamilyCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    KatashikiCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
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
                    table.PrimaryKey("PK_IF_FQF3MM01", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IF_FQF3MM01");
        }
    }
}
