using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class drmihppartlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvDrmPartList",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SupplierCd = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    MaterialCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    MaterialSpec = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BoxQty = table.Column<int>(type: "int", nullable: true),
                    PartCode = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    FirstDayProduct = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastDayProduct = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sourcing = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Cutting = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Packing = table.Column<int>(type: "int", nullable: true),
                    SheetWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YiledRation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_InvDrmPartList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvIhpPartGrade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    IhpPartId = table.Column<long>(type: "bigint", nullable: true),
                    UsageQty = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_InvIhpPartGrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvIhpPartList",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SupplierCd = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PartCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PartSpec = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartSize = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Sourcing = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Cutting = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Packing = table.Column<int>(type: "int", nullable: true),
                    SheetWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YiledRation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_InvIhpPartList", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvDrmPartList");

            migrationBuilder.DropTable(
                name: "InvIhpPartGrade");

            migrationBuilder.DropTable(
                name: "InvIhpPartList");
        }
    }
}
