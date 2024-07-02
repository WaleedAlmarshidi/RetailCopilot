using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailCopilot.Migrations
{
    /// <inheritdoc />
    public partial class FromPartnersToUsersWithPosSalesV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_PosSales_Users_EmployeeExternalId",
            //     table: "PosSales");

            migrationBuilder.DropIndex(
                name: "IX_Partners_ExternalId",
                table: "Partners");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeExternalId",
                table: "PosSales",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Partners",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Partners",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ExternalId",
                table: "AspNetUsers",
                column: "ExternalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PosSales_Users_EmployeeExternalId",
                table: "PosSales",
                column: "EmployeeExternalId",
                principalTable: "AspNetUsers",
                principalColumn: "ExternalId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosSales_Users_EmployeeExternalId",
                table: "PosSales");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ExternalId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeExternalId",
                table: "PosSales",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldDefaultValue: "0");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Partners",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Partners",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_ExternalId",
                table: "Partners",
                column: "ExternalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PosSales_Users_EmployeeExternalId",
                table: "PosSales",
                column: "EmployeeExternalId",
                principalTable: "AspNetUsers",
                principalColumn: "ExternalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
