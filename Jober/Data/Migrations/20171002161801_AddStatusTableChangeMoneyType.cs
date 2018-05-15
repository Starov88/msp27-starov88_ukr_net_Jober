using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class AddStatusTableChangeMoneyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Services",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TotalCost",
                table: "Orders",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusIdId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_StatusId",
                table: "Services",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId1",
                table: "Orders",
                column: "StatusId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusIdId",
                table: "Orders",
                column: "StatusIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Statuses_StatusId1",
                table: "Orders",
                column: "StatusId1",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Statuses_StatusIdId",
                table: "Orders",
                column: "StatusIdId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Statuses_StatusId",
                table: "Services",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_StatusId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_StatusIdId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Statuses_StatusId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Services_StatusId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusIdId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "StatusId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusIdId",
                table: "Orders");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Services",
                type: "money",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "money",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Orders",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
