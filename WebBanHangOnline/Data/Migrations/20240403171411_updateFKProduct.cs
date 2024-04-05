using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanHangOnline.Data.Migrations
{
    public partial class updateFKProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Product_tb_ProductCategory_ProductCategoryId",
                table: "tb_Product");

            migrationBuilder.AlterColumn<int>(
                name: "ProductCategoryId",
                table: "tb_Product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Product_tb_ProductCategory_ProductCategoryId",
                table: "tb_Product",
                column: "ProductCategoryId",
                principalTable: "tb_ProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Product_tb_ProductCategory_ProductCategoryId",
                table: "tb_Product");

            migrationBuilder.AlterColumn<int>(
                name: "ProductCategoryId",
                table: "tb_Product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Product_tb_ProductCategory_ProductCategoryId",
                table: "tb_Product",
                column: "ProductCategoryId",
                principalTable: "tb_ProductCategory",
                principalColumn: "Id");
        }
    }
}
