using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailCopilot.Migrations
{
    /// <inheritdoc />
    public partial class FromPartnersToUsersWithPosSalesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PosSales_EmployeeExternalId",
                table: "PosSales",
                column: "EmployeeExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosSales_Users_EmployeeExternalId",
                table: "PosSales");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_PosSales_LastPosSaleId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DailyAggregatedPosSales");

            migrationBuilder.DropTable(
                name: "Inquiries");

            migrationBuilder.DropTable(
                name: "PosOrders");

            migrationBuilder.DropTable(
                name: "ShopVisitsCount");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "UserAuthorizedPointOfSales");

            migrationBuilder.DropTable(
                name: "ViolationRecords");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Violations");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PosSales");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Pos");

            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
