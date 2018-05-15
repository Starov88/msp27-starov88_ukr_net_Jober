using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class ServiceWorkerToCategoryWorker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Districts_DistrictId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AspNetUsers_UserId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "ServiceWorkers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_UserId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "IsWorker",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                table: "Workers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Workers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerSettingJson",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryWorkers",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryWorkers", x => new { x.CategoryId, x.WorkerId });
                    table.ForeignKey(
                        name: "FK_CategoryWorkers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkerId",
                table: "AspNetUsers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryWorkers_WorkerId",
                table: "CategoryWorkers",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Workers_WorkerId",
                table: "AspNetUsers",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Districts_DistrictId",
                table: "Workers",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Workers_WorkerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Districts_DistrictId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "CategoryWorkers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "WorkerSettingJson",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Workers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                table: "Workers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWorker",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ServiceWorkers",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceWorkers", x => new { x.ServiceId, x.WorkerId });
                    table.ForeignKey(
                        name: "FK_ServiceWorkers_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_UserId",
                table: "Workers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceWorkers_WorkerId",
                table: "ServiceWorkers",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Districts_DistrictId",
                table: "Workers",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AspNetUsers_UserId",
                table: "Workers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
