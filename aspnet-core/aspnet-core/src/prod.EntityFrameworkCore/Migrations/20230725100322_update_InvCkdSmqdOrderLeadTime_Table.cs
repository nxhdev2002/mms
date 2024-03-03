using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateInvCkdSmqdOrderLeadTimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvCkdSmqdOrderLeadTime",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cfc = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ExpCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sea = table.Column<int>(type: "int", nullable: true),
                    Air = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_InvCkdSmqdOrderLeadTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvCkdSmqdOrderLeadTime_T",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SupplierNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cfc = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ExpCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sea = table.Column<int>(type: "int", nullable: true),
                    Air = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_InvCkdSmqdOrderLeadTime_T", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvCkdSmqdOrderLeadTime");

            migrationBuilder.DropTable(
                name: "InvCkdSmqdOrderLeadTime_T");
        }
    }
}
