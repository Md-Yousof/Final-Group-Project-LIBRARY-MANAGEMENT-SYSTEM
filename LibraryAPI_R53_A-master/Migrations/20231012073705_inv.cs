using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI_R53_A.Migrations
{
    public partial class inv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fines");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "SubscriptionUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "SubscriptionUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "SubscriptionUsers");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "SubscriptionUsers");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fines",
                columns: table => new
                {
                    FineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowedBookId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fines", x => x.FineId);
                    table.ForeignKey(
                        name: "FK_Fines_BorrowedBooks_BorrowedBookId",
                        column: x => x.BorrowedBookId,
                        principalTable: "BorrowedBooks",
                        principalColumn: "BorrowedBookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fines_BorrowedBookId",
                table: "Fines",
                column: "BorrowedBookId");
        }
    }
}
