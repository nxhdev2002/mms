using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod.Migrations
{
    /// <inheritdoc />
    public partial class updateMstCmmMMValidationResultTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resultfield",
                table: "MstCmmMMValidationResult",
                newName: "ResultField");

            migrationBuilder.RenameColumn(
                name: "Lastvalidationid",
                table: "MstCmmMMValidationResult",
                newName: "LastValidationId");

            migrationBuilder.RenameColumn(
                name: "Lastvalidationdatetime",
                table: "MstCmmMMValidationResult",
                newName: "LastValidationDatetime");

            migrationBuilder.RenameColumn(
                name: "Isactive",
                table: "MstCmmMMValidationResult",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Expectedresult",
                table: "MstCmmMMValidationResult",
                newName: "ExpectedResult");

            migrationBuilder.RenameColumn(
                name: "Errormessage",
                table: "MstCmmMMValidationResult",
                newName: "ErrorMessage");

            migrationBuilder.AlterColumn<long>(
                name: "RuleId",
                table: "MstCmmMMValidationResult",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MateriaId",
                table: "MstCmmMMValidationResult",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "LastValidationId",
                table: "MstCmmMMValidationResult",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResultField",
                table: "MstCmmMMValidationResult",
                newName: "Resultfield");

            migrationBuilder.RenameColumn(
                name: "LastValidationId",
                table: "MstCmmMMValidationResult",
                newName: "Lastvalidationid");

            migrationBuilder.RenameColumn(
                name: "LastValidationDatetime",
                table: "MstCmmMMValidationResult",
                newName: "Lastvalidationdatetime");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "MstCmmMMValidationResult",
                newName: "Isactive");

            migrationBuilder.RenameColumn(
                name: "ExpectedResult",
                table: "MstCmmMMValidationResult",
                newName: "Expectedresult");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "MstCmmMMValidationResult",
                newName: "Errormessage");

            migrationBuilder.AlterColumn<string>(
                name: "RuleId",
                table: "MstCmmMMValidationResult",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MateriaId",
                table: "MstCmmMMValidationResult",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Lastvalidationid",
                table: "MstCmmMMValidationResult",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
