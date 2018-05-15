using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class addforeginkeyOrderCategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CategoryId",
                table: "Orders",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Categories_CategoryId",
                table: "Orders",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Categories_CategoryId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CategoryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Orders");
        }
    }
}
