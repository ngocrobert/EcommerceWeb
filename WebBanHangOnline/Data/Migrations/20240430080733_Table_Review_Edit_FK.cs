using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanHangOnline.Data.Migrations
{
    public partial class Table_Review_Edit_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tb_Review_ProductId",
                table: "tb_Review",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Review_tb_Product_ProductId",
                table: "tb_Review",
                column: "ProductId",
                principalTable: "tb_Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Review_tb_Product_ProductId",
                table: "tb_Review");

            migrationBuilder.DropIndex(
                name: "IX_tb_Review_ProductId",
                table: "tb_Review");
        }
    }
}
