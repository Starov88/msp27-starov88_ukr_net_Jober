using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class ChangeOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_StatusId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_StatusIdId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Workers_WorkerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusIdId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusIdId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "WorkerId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Statuses_StatusId",
                table: "Orders",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Workers_WorkerId",
                table: "Orders",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_StatusId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Workers_WorkerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "WorkerId",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId1",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusIdId",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

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
                name: "FK_Orders_Workers_WorkerId",
                table: "Orders",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
