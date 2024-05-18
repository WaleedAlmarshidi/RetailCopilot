using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailCopilot.Migrations
{
    /// <inheritdoc />
    public partial class AddLoyaltyBadge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionRate",
                table: "ShopVisitsCount");

            migrationBuilder.AddColumn<int>(
                name: "LoyaltyBadge",
                table: "Contacts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoyaltyBadge",
                table: "Contacts");

            migrationBuilder.AddColumn<double>(
                name: "ConversionRate",
                table: "ShopVisitsCount",
                type: "float",
                nullable: true);
        }
    }
}
