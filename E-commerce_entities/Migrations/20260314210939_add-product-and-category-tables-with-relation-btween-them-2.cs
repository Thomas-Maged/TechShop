using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_entities.Migrations
{
    /// <inheritdoc />
    public partial class addproductandcategorytableswithrelationbtweenthem2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CretedAt",
                table: "products",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "products",
                newName: "CretedAt");
        }
    }
}
