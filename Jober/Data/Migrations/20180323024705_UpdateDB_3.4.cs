using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class UpdateDB_34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Workers_WorkerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Workers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsWorker",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_UserId",
                table: "Workers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AspNetUsers_UserId",
                table: "Workers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AspNetUsers_UserId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_UserId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "IsWorker",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkerId",
                table: "AspNetUsers",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Workers_WorkerId",
                table: "AspNetUsers",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
