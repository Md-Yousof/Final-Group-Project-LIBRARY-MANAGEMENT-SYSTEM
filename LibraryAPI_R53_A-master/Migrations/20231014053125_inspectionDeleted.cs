using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI_R53_A.Migrations
{
    public partial class inspectionDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.AddColumn<decimal>(
                name: "MiscellaneousFines",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MiscellaneousFines",
                table: "Invoices");

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    InspectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookCopyId = table.Column<int>(type: "int", nullable: false),
                    BorrowBookId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.InspectionId);
                    table.ForeignKey(
                        name: "FK_Inspections_BorrowedBooks_BorrowBookId",
                        column: x => x.BorrowBookId,
                        principalTable: "BorrowedBooks",
                        principalColumn: "BorrowedBookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspections_Copies_BookCopyId",
                        column: x => x.BookCopyId,
                        principalTable: "Copies",
                        principalColumn: "BookCopyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_BookCopyId",
                table: "Inspections",
                column: "BookCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_BorrowBookId",
                table: "Inspections",
                column: "BorrowBookId");
        }
    }
}
