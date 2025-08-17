// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSkibiditeca.Web.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyLoanStatusAndFineType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fines_FineTypes_FineTypeId",
                table: "Fines");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_LoanStatuses_LoanStatusId",
                table: "Loans");

            migrationBuilder.DropTable(
                name: "FineTypes");

            migrationBuilder.DropTable(
                name: "LoanStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoanStatusId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Fines_FineTypeId",
                table: "Fines");

            migrationBuilder.DropColumn(
                name: "FineTypeId",
                table: "Fines");

            migrationBuilder.RenameColumn(
                name: "LoanStatusId",
                table: "Loans",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Loans",
                newName: "LoanStatusId");

            migrationBuilder.AddColumn<int>(
                name: "FineTypeId",
                table: "Fines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FineTypes",
                columns: table => new
                {
                    FineTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FineTypes", x => x.FineTypeId);
                });

            migrationBuilder.CreateTable(
                name: "LoanStatuses",
                columns: table => new
                {
                    LoanStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanStatuses", x => x.LoanStatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanStatusId",
                table: "Loans",
                column: "LoanStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Fines_FineTypeId",
                table: "Fines",
                column: "FineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanStatuses_Name",
                table: "LoanStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_FineTypes_FineTypeId",
                table: "Fines",
                column: "FineTypeId",
                principalTable: "FineTypes",
                principalColumn: "FineTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_LoanStatuses_LoanStatusId",
                table: "Loans",
                column: "LoanStatusId",
                principalTable: "LoanStatuses",
                principalColumn: "LoanStatusId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
