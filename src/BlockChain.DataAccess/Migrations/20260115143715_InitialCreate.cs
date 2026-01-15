using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockChain.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockchainSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Chain = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Json = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockchainSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainSnapshots_Chain_CreatedAt",
                table: "BlockchainSnapshots",
                columns: new[] { "Chain", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockchainSnapshots");
        }
    }
}
