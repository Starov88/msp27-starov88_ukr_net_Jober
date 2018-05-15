using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class FixStatusrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Statuses_StatusId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_StatusId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Services");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Services",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_StatusId",
                table: "Services",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Statuses_StatusId",
                table: "Services",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
