using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailCopilot.Migrations
{
    /// <inheritdoc />
    public partial class FromPartnersToUsersWithPosSalesV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_PosSales_Partners_PartnerId",
            //     table: "PosSales");

            migrationBuilder.DropTable(
                name: "Partners");

            // migrationBuilder.DropIndex(
            //     name: "IX_PosSales_PartnerId",
            //     table: "PosSales");

            // migrationBuilder.DropColumn(
            //     name: "PartnerId",
            //     table: "PosSales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "PosSales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PosSales_PartnerId",
                table: "PosSales",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PosSales_Partners_PartnerId",
                table: "PosSales",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id");
        }
    }
}
