using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jober.Data.Migrations
{
    public partial class AddImgFilePathColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgFilePath",
                table: "Services",
                type: "text",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgFilePath",
                table: "Categories",
                type: "text",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgFilePath",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ImgFilePath",
                table: "Categories");
        }
    }
}
