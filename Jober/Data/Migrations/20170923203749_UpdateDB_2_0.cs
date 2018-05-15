using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class UpdateDB_2_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Quantity",
                table: "Orders",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Orders",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}
