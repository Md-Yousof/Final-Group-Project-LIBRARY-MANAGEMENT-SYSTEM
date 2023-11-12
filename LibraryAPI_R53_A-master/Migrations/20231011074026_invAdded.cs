using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI_R53_A.Migrations
{
    public partial class invAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AspNetUsers_ApplicationUserId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_SubscriptionPlans_SubId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ApplicationUserId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SubId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SubId",
                table: "Invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Invoices",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ApplicationUserId",
                table: "Invoices",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubId",
                table: "Invoices",
                column: "SubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AspNetUsers_ApplicationUserId",
                table: "Invoices",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_SubscriptionPlans_SubId",
                table: "Invoices",
                column: "SubId",
                principalTable: "SubscriptionPlans",
                principalColumn: "SubscriptionPlanId");
        }
    }
}
