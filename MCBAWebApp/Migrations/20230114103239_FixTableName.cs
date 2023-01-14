using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCBAWebApp.Migrations
{
    /// <inheritdoc />
    public partial class FixTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillPays_Accounts_AccountNumber",
                table: "BillPays");

            migrationBuilder.DropForeignKey(
                name: "FK_BillPays_Payees_PayeeID",
                table: "BillPays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillPays",
                table: "BillPays");

            migrationBuilder.RenameTable(
                name: "BillPays",
                newName: "BillPayments");

            migrationBuilder.RenameIndex(
                name: "IX_BillPays_PayeeID",
                table: "BillPayments",
                newName: "IX_BillPayments_PayeeID");

            migrationBuilder.RenameIndex(
                name: "IX_BillPays_AccountNumber",
                table: "BillPayments",
                newName: "IX_BillPayments_AccountNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillPayments",
                table: "BillPayments",
                column: "BillPayID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillPayments_Accounts_AccountNumber",
                table: "BillPayments",
                column: "AccountNumber",
                principalTable: "Accounts",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillPayments_Payees_PayeeID",
                table: "BillPayments",
                column: "PayeeID",
                principalTable: "Payees",
                principalColumn: "PayeeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillPayments_Accounts_AccountNumber",
                table: "BillPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_BillPayments_Payees_PayeeID",
                table: "BillPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillPayments",
                table: "BillPayments");

            migrationBuilder.RenameTable(
                name: "BillPayments",
                newName: "BillPays");

            migrationBuilder.RenameIndex(
                name: "IX_BillPayments_PayeeID",
                table: "BillPays",
                newName: "IX_BillPays_PayeeID");

            migrationBuilder.RenameIndex(
                name: "IX_BillPayments_AccountNumber",
                table: "BillPays",
                newName: "IX_BillPays_AccountNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillPays",
                table: "BillPays",
                column: "BillPayID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillPays_Accounts_AccountNumber",
                table: "BillPays",
                column: "AccountNumber",
                principalTable: "Accounts",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillPays_Payees_PayeeID",
                table: "BillPays",
                column: "PayeeID",
                principalTable: "Payees",
                principalColumn: "PayeeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
