using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class UpdateDB_45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Districts_DistrictId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_DistrictId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Workers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Workers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_DistrictId",
                table: "Workers",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Districts_DistrictId",
                table: "Workers",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
