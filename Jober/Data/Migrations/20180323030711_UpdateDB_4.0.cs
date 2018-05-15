using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class UpdateDB_40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Services_ServiceId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ServiceId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ServiceId",
                table: "Orders",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Services_ServiceId",
                table: "Orders",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
