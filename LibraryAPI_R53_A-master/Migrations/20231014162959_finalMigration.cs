using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI_R53_A.Migrations
{
    public partial class finalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_SubscriptionPlans_SubscriptionPlanId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SubscriptionPlanId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "Invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubscriptionPlanId",
                table: "Invoices",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_SubscriptionPlans_SubscriptionPlanId",
                table: "Invoices",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "SubscriptionPlanId");
        }
    }
}
