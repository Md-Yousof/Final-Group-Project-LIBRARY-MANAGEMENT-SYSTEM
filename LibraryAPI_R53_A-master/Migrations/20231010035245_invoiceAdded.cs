using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI_R53_A.Migrations
{
    public partial class invoiceAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Inspections");

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyFee",
                table: "SubscriptionPlans",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubId = table.Column<int>(type: "int", nullable: true),
                    Payment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Refund = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fine = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BorrowedBookId = table.Column<int>(type: "int", nullable: true),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_BorrowedBooks_BorrowedBookId",
                        column: x => x.BorrowedBookId,
                        principalTable: "BorrowedBooks",
                        principalColumn: "BorrowedBookId");
                    table.ForeignKey(
                        name: "FK_Invoices_BorrowedBooks_BorrowId",
                        column: x => x.BorrowId,
                        principalTable: "BorrowedBooks",
                        principalColumn: "BorrowedBookId");
                    table.ForeignKey(
                        name: "FK_Invoices_SubscriptionPlans_SubId",
                        column: x => x.SubId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "SubscriptionPlanId");
                    table.ForeignKey(
                        name: "FK_Invoices_SubscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "SubscriptionPlanId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ApplicationUserId",
                table: "Invoices",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BorrowedBookId",
                table: "Invoices",
                column: "BorrowedBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BorrowId",
                table: "Invoices",
                column: "BorrowId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubId",
                table: "Invoices",
                column: "SubId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubscriptionPlanId",
                table: "Invoices",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UserId",
                table: "Invoices",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropColumn(
                name: "MonthlyFee",
                table: "SubscriptionPlans");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Inspections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
