using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateInvPIOTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvPioPartList",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullModel = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ProdSfx = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    MktCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_InvPioPartList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvPIOStock",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PartId = table.Column<long>(type: "bigint", nullable: true),
                    MktCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    TransId = table.Column<long>(type: "bigint", nullable: true),
                    TransDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true),
                    VinNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Shop = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CartType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InteriorColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_InvPIOStock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvPIOStockRundown",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PartId = table.Column<long>(type: "bigint", nullable: true),
                    MktCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    TransId = table.Column<long>(type: "bigint", nullable: true),
                    TransDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true),
                    VinNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Shop = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CartType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InteriorColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_InvPIOStockRundown", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvPIOStockRundownTransaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PartId = table.Column<long>(type: "bigint", nullable: true),
                    MktCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    TransId = table.Column<long>(type: "bigint", nullable: true),
                    TransDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true),
                    VinNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Shop = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CartType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InteriorColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_InvPIOStockRundownTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvPIOStockTransaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PartId = table.Column<long>(type: "bigint", nullable: true),
                    MktCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    TransId = table.Column<long>(type: "bigint", nullable: true),
                    TransDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true),
                    VinNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Shop = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CartType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InteriorColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_InvPIOStockTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstInvPIOEmail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    To = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BodyMess = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    SupplierCd = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_MstInvPIOEmail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstInvPIOPartType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_MstInvPIOPartType", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvPioPartList");

            migrationBuilder.DropTable(
                name: "InvPIOStock");

            migrationBuilder.DropTable(
                name: "InvPIOStockRundown");

            migrationBuilder.DropTable(
                name: "InvPIOStockRundownTransaction");

            migrationBuilder.DropTable(
                name: "InvPIOStockTransaction");

            migrationBuilder.DropTable(
                name: "MstInvPIOEmail");

            migrationBuilder.DropTable(
                name: "MstInvPIOPartType");
        }
    }
}
