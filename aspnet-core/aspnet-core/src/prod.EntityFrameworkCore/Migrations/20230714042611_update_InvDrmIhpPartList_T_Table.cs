using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateInvDrmIhpPartListTTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvDrmIhpPartList_T",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SupplierType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SupplierDrm = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    MaterialCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    MaterialSpec = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BoxQty = table.Column<int>(type: "int", nullable: true),
                    SupplierIhp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ModelIhp = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PartCode = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Spec = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    FirstDayProduct = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastDayProduct = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    UsageQty = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_InvDrmIhpPartList_T", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvDrmIhpPartList_T");
        }
    }
}
