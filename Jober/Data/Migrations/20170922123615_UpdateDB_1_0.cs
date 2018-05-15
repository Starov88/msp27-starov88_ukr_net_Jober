using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class UpdateDB_1_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Orders_OrderId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_AspNetUsers_UserId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_UserId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EvaluationBad",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "EvaluationGood",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Services",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "text",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Orders",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Orders",
                type: "text",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsResidence",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Districts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Orders",
                type: "datetime",
                nullable: false,
                computedColumnSql: "GETDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    EvaluationBad = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EvaluationGood = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workers_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceWorkers",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkerId",
                table: "AspNetUsers",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceWorkers_WorkerId",
                table: "ServiceWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_CityId",
                table: "Workers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_DistrictId",
                table: "Workers",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Workers_WorkerId",
                table: "AspNetUsers",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Workers_WorkerId",
                table: "Orders",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Workers_WorkerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Workers_WorkerId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ServiceWorkers");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Districts_CityId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsResidence",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Services",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationBad",
                table: "Services",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationGood",
                table: "Services",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Services",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Orders",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldComputedColumnSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_UserId",
                table: "Services",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderId",
                table: "Orders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Orders_OrderId",
                table: "Orders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_AspNetUsers_UserId",
                table: "Services",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
