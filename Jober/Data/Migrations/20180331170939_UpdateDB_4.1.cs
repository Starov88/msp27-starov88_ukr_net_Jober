using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class UpdateDB_41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Orders");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Services",
                type: "real",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Categories",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Categories");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Services",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Orders",
                type: "datetime",
                nullable: false,
                computedColumnSql: "GETDATE()");
        }
    }
}
