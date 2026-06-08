using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_entities.Migrations
{
    /// <inheritdoc />
    public partial class addingimagecolinproducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_Items_Orders_OrderID",
                table: "order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_order_Items_products_ProductID",
                table: "order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_products_Categories_CategoryID",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_Items",
                table: "order_Items");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "order_Items",
                newName: "Order_Items");

            migrationBuilder.RenameIndex(
                name: "IX_products_CategoryID",
                table: "Products",
                newName: "IX_Products_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_order_Items_ProductID",
                table: "Order_Items",
                newName: "IX_Order_Items_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_order_Items_OrderID",
                table: "Order_Items",
                newName: "IX_Order_Items_OrderID");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_Items",
                table: "Order_Items",
                column: "OrderItemID");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "Name",
                value: "ADMIN");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "Name",
                value: "USER");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Orders_OrderID",
                table: "Order_Items",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Products_ProductID",
                table: "Order_Items",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Orders_OrderID",
                table: "Order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Products_ProductID",
                table: "Order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_Items",
                table: "Order_Items");

            migrationBuilder.DropColumn(
                name: "image",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");

            migrationBuilder.RenameTable(
                name: "Order_Items",
                newName: "order_Items");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryID",
                table: "products",
                newName: "IX_products_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Items_ProductID",
                table: "order_Items",
                newName: "IX_order_Items_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Items_OrderID",
                table: "order_Items",
                newName: "IX_order_Items_OrderID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_Items",
                table: "order_Items",
                column: "OrderItemID");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "Name",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "Name",
                value: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_order_Items_Orders_OrderID",
                table: "order_Items",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_Items_products_ProductID",
                table: "order_Items",
                column: "ProductID",
                principalTable: "products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_Categories_CategoryID",
                table: "products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
