using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class CreateMstCmmCheckRulesT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstCmmMMCheckingRule_T",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RuleDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    RuleItem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field1Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field1Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field2Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field2Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field3Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field3Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field4Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field4Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field5Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Field5Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Option = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Resultfield = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Expectedresult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Checkoption = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Errormessage = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
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
                    table.PrimaryKey("PK_MstCmmMMCheckingRule_T", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstCmmMMCheckingRule_T");
        }
    }
}
