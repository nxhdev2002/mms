using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateMstCmmBusinessParterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstCmmBusinessParter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nation = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BusinessPartnerCategory = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BusinessPartnerGroup = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    BusinessPartnerRole = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    BusinessPartnerCd = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EmailAddress1 = table.Column<string>(type: "nvarchar(241)", maxLength: 241, nullable: true),
                    SuppSearcgTerm = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BusinessPartnerName1 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    BusinessPartnerName2 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    BusinessPartnerName3 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    BusinessPartnerName4 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Address3 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    District = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    City = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PostalCd = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FaxNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TaxNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TaxCate = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PaymentMethodCd = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PaymentMethodNm = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PaymentTermCd = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PaymentTermNm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    TypeOfIndustry = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PreviousMasterRecordNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TextIdTitle = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UniqueBankId = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    SuppBankKey = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SuppBankCountry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    SuppAccount = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    AccountHolder = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Accname = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PartnerBankName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    StatusFlagAb = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    StatusFlagCb = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    StatusFlagAd = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    StatusFlagCd = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_MstCmmBusinessParter", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstCmmBusinessParter");
        }
    }
}
